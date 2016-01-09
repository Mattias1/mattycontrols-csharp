import re

# The filenames (hardcoded, I know)
filename_from = 'MattyControls.template.cs'
filename_to = 'MattyControls.cs'



# Easy regex matches
def lineIs(line, search)
    pattern = re.compile('^\s*' + search + '\s*$')
    pattern.match(line)

specialPattern = re.compile('^\s*// --')
def lineIsSpecial(line)
    specialPattern.match(line)



# The function that does all the parsing and compiling
def compile(line, fto):
    fto.write(line)



# Handle the file IO
with open(filename_to, 'w') as fto:
    with open(filename_from) as ffrom:
        for line in ffrom:
            compile(line, fto)

print('Done')
