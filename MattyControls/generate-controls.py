import re

# The filenames (hardcoded, I know)
filename_from = 'MattyControls.template.cs'
filename_to = 'MattyControls.cs'



# Easy regex matches
def lineIs(line : str, search : str):
    pattern = re.compile('^\s*' + search + '\s*$')
    return bool(pattern.match(line))

def lineBeginsWith(line : str, search : str):
    pattern = re.compile('^\s*' + search)
    return bool(pattern.match(line))

specialPattern = re.compile('^\s*// --')
def lineIsSpecial(line : str):
    return bool(specialPattern.match(line))



# The control object
class Control:
    def __init__(self, typename : str, basetype : str):
        self.typename = typename
        self.basetype = basetype
        self.constructor = ''

    def writeToFile(self, fto, controlcopy):
        def w(s = ''):
            fto.write(s + '\n')

        w()
        w('    public class {} : {}'.format(self.typename, self.basetype))
        w('    {')
        w(self.constructor)
        w(controlcopy)
        w('    }')



# The actual compiler
class FlatCompiler:
    def __init__(self):
        self.status = 0
        self.lines = []
        self.controlcopy = ''
        self.controls = []

    # The function that does all the parsing and compiling, line by line (foreach'es are already dealt with)
    def compile(self, line : str, fto):
        # normal (read and write directly)
        if self.status == 0:
            self._normalMode()

        # Parse the types and corresponding constructor function(s)
        elif self.status == 1:
            self._parseTypesMode()

        # Copy the control methods
        elif self.status == 2:
            self._copyControlsMode()

    def _normalMode(self):
        if lineIsSpecial(line):
            # Ok, pay attention now
            if lineIs(line, '// -- begin types --'):
                print('Begin types ...')
                self.status = 1
            elif lineIs(line, '// -- begin control copy --'):
                print('Begin control copy ...')
                self.status = 2
            elif lineIs(line, '// -- write controls --'):
                print('Write controls ...')
                for c in self.controls:
                    c.writeToFile(fto, self.controlcopy)
        else:
            # Read and write directly
            fto.write(line)

    def _parseTypesMode(self):
        if lineIsSpecial(line):
            # Finish the previous control
            try:
                self.controls[-1].constructor = ''.join(self.lines)
                self.lines = []
            except IndexError:
                pass

            # If there is no new control, go back to normal
            if lineIs(line, '// -- end types --'):
                self.status = 0
                self.lines = []
            else:
                # Start a new control
                m = re.search('^\s*// -- (\w+) : (\w+)\s*$', line)
                self.controls.append(Control(m.group(1), m.group(2)))
        else:
            # Save the constructor functions
            self.lines.append(line)

    def _copyControlsMode(self):
        if lineIs(line, '// -- end control copy --'):
            # End of the control methods
            self.controlcopy = ''.join(self.lines)
            self.status = 0
            self.lines = []
        else:
            # Add to the control methods
            self.lines.append(line)


# The compiler that 'executes' the loop before we give the template file to the actual compiler
class LoopPreCompiler:
    def __init__(self):
        self.isForeach = False # This means I don't support nested foreach'es
        self.lines = None
        self.loopVariables = None
        self.loopList = None

    # The function that duplicates the lines surrounded by a foreach marker
    def compile(self, result, line):
        # Detect foreach
        if lineBeginsWith(line, '// -- foreach '):
            self._readForeach()
            return

        # Handle foreach
        if lineIs(line, '// -- endforeach --'):
            self._writeForeach(result)
            return

        # Copy the lines we want to loop
        if self.isForeach:
            self.lines.append(line)
            return

        # Nothing special
        result.append(line)

    def _readForeach(self):
        m = re.search('^\s*// -- foreach (.*) in \[(.+)\] --\s*$', line)
        self.loopVariables = [s.strip() for s in m.group(1).split(',')]
        self.loopList = eval("[{}]".format(m.group(2)))

        self.isForeach = True
        self.lines = []

    def _writeForeach(self, result):
        for things in self.loopList:
            replacements = self._arrify(things)
            for line in self.lines:
                for name, content in zip(self.loopVariables, replacements):
                    line = line.replace('{{' + name + '}}', content)

                result.append(line)

        self.isForeach = False

    def _arrify(self, things):
        try:
            things[0]
        except:
            things = [things]
        return things



# Handle the file IO
with open(filename_to, 'w') as fto:
    fto.write('//\n')
    fto.write('// NOTICE: This file is generated. Do not edit this file manually, please edit the template instead.\n')
    fto.write('//\n')

    with open(filename_from) as ffrom:
        sourceLines = []
        loopCompiler = LoopPreCompiler()
        for line in ffrom:
            loopCompiler.compile(sourceLines, line)

        flatCompiler = FlatCompiler()
        for line in sourceLines:
            flatCompiler.compile(line, fto)

print('Done')
