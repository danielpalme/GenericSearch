grammar SearchGrammar;

/*
 * Parser Rules
 */

prog :                    orExpression ;

orExpression :            andExpression
                          | orExpression op=('or'|'|') andExpression
                          ;

andExpression :           primaryExpression
                          | andExpression primaryExpression
                          | andExpression op=('and'|'&') primaryExpression
                          ;

primaryExpression :       TERM
                          | negatedExpression
                          | parenthesizedExpression
                          | phraseExpression
                          ;


phraseExpression :        '"' TERM '"' ;

negatedExpression :       op=('not'|'-') primaryExpression ;

parenthesizedExpression : '(' orExpression ')';

/*
 * Lexer Rules
 */

TERM :                    [a-zA-Z0-9$_]+;