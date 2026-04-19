"""
Script utilizado para alterar a cor do SVG gerado pelo dbdiagram.
"""
import argparse
import sys
import re


def main(options):
    with open(options.infile, 'r') as f:
        content = f.read()
    
    if '#316896' not in content:
        print('O arquivo já parece estar corrigido!')
        sys.exit(0)

    content = content.replace('#316896', f'#{options.color}')
    print(f'Cor alterada para: {options.color}')
    with open(options.infile, 'w') as f:
        f.write(content)

def parse_options():
    parser = argparse.ArgumentParser()

    parser.add_argument('-c', '--color', default='333333', help='Cor para substituir.')
    parser.add_argument('infile', help='Arquivo svg para corrigir.')

    options = parser.parse_args()

    if not options.infile.endswith('.svg'):
        print('O arquivo deve ser um svg!')
        sys.exit(1)

    match = re.match(r'#?([A-Fa-f0-9]{3}|[A-Fa-f0-9]{6})', options.color)
    if not match:
        print('Insira uma cor válida')
        sys.exit(1)
    
    options.color = match.group()

    main(options)

if __name__ == '__main__':
    parse_options()