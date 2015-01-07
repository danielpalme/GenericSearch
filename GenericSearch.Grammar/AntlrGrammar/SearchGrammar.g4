grammar SearchGrammar;

/*
 * Parser Rules
 */

prog :                    orExpression ;

orExpression :            andExpression
                          | orExpression WS op=('or'|'|') WS andExpression
                          ;

andExpression :           primaryExpression
                          | andExpression WS primaryExpression
                          | andExpression WS op=('and'|'&') WS primaryExpression
                          ;

primaryExpression :       TERM
                          | phraseExpression
                          | negatedExpression
                          | parenthesizedExpression
                          ;


phraseExpression :        '"' ~('"')+ '"' ;

negatedExpression :       op=('not'|'-') primaryExpression
                          | op=('not'|'-') WS primaryExpression;

parenthesizedExpression : '(' orExpression ')';

/*
 * Lexer Rules
 */

TERM :                   
    (
        [a-zA-Z0-9]
        |
        /*
            matches characters considered Unicode letters.
            taken from: http://www.uco.es/~ma1fegan/Comunes/manuales/pl/ANTLR/ANTLR-INGLES.pdf
        */
        '\u0024' |
        ['\u0041-\u005a'] |
        '\u005f' |
        ['\u0061-\u007a'] |
        ['\u00c0-\u00d6'] |
        ['\u00d8-\u00f6'] |
        ['\u00f8-\u00ff'] |
        ['\u0100-\u1fff'] |
        ['\u3040-\u318f'] |
        ['\u3300-\u337f'] |
        ['\u3400-\u3d2d'] |
        ['\u4e00-\u9fff'] |
        ['\uf900-\ufaff']
    )+
;

WS :
    (' ' | '\t'| '\f')+;