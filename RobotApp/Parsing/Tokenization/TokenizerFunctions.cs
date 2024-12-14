﻿using LanguageExt;
using static LanguageExt.Prelude;
using RobotApp.Parsing.DataTypes;
using RobotApp.Parsing.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static RobotApp.Parsing.Tokenization.StringWithPointerFunctions;
using LanguageExt.ClassInstances;
using LanguageExt.TypeClasses;
using System.Runtime.CompilerServices;
using static RobotApp.Parsing.Utility.AltFunctions;
using static RobotApp.Parsing.DataTypes.DocAndChunkFunctions;

namespace RobotApp.Parsing.Tokenization;

public static class TokenizerFunctions
{
    public static Either<TokenizeFail, Doc<TokenLine>> TokenizeDocument(string rawDoc)
    {
        return Chunker(rawDoc).Sequence(textline => Tokenize(textline));
    }

    public static Doc<TextLine> Chunker(string rawDoc)
    {
        var chunks = rawDoc.Split(Environment.NewLine)
            .Map((i, c) => new TextLine(text: c, lineNumber: i + 1))
            .Split(txtLine => txtLine.Text.Trim() == "")
            .Filter(chunk => chunk.Any())
            .Map(lines => new Chunk<TextLine>(lines.ToList()))
            .ToList();

        var textDoc = new Doc<TextLine>(chunks);
        return textDoc;
    }

    public static string TokenizeFailMesg =>
        @"Unable to do initial parsing of line. Please only use seqeunces of 1 or more 
          alphabetic characters or seqeunces of 1 or more numeric characters seperated by
          single spaces";

    public static Either<TokenizeFail, TokenLine> Tokenize(TextLine txtLine)
    {
        var stringWithPtr = new StringWithPointer(txtLine.Text, 0);
        var tokenizeChain = 
            AltChain<StringWithPointer, WithStringPointerState<Token>>
            (new (){ TokenizeWord,
                     TokenizeNumber,
                     TokenizeStandaloneLetter,
                     TokenizeNumberxNumber });

        return RecurseTokenizer(tokenizeChain, stringWithPtr).Match(
                   Some: seq => Right(
                       seq.TakeWhile(tok => tok.TokenType != TokenType.End).ToList()),
                   None: Left<TokenizeFail, List<Token>>(
                       new TokenizeFail(TokenizeFailMesg, stringWithPtr.Value, txtLine.LineNumber))
               ).Map(
                   listToks => new TokenLine(txtLine.LineNumber, listToks)
               );
    }

    //TODO: Make this Lazy so we don't have to include "isEnd" and we can just
    //      handle the End case with the above TakeWith
    private static Option<Seq<Token>> RecurseTokenizer(
        Func<StringWithPointer, Option<WithStringPointerState<Token>>> tokeizer,
        StringWithPointer strP)
    {
        return from wrappedTok in tokeizer(strP)
               let isEnd = wrappedTok.Value.TokenType == TokenType.End
               from rest in isEnd ? Some<Seq<Token>>(new()) 
                                  : RecurseTokenizer(tokeizer, wrappedTok.StringWithPointer)
               select wrappedTok.Value.Cons(rest);
    }

    //Match with strings starting with " kjkdokf", "jdciojcdij", " kcmcm "
    public static Option<WithStringPointerState<Token>> TokenizeWord(StringWithPointer strP) =>
        GetRestOfString(strP).Match(

                Some: restStr => Regex.Match(restStr,
                    "^([a-z]{2,} )|^( [a-z]{2,} )|^([a-z]{2,})$",
                    RegexOptions.IgnoreCase)
                    .IfMatchSuccessCreateTokenWithState(strP, TokenType.Word),

                None: () => new WithStringPointerState<Token>(
                    new Token("", TokenType.End), strP
                 )
            );


    //Match with strings starting with " 278468", "9900", " 14 "
    public static Option<WithStringPointerState<Token>> TokenizeNumber(StringWithPointer strP) =>
        GetRestOfString(strP).Match(

                Some: restStr => Regex.Match(restStr,
                    "^([0-9]+ )|^( [0-9]+ )|^([0-9]+)$",
                    RegexOptions.IgnoreCase)
                    .IfMatchSuccessCreateTokenWithState(strP, TokenType.Number),

                None: new WithStringPointerState<Token>(
                    new Token("", TokenType.End), strP
                 )
            );

    //Match with strings starting with " a", "K", " J "
    public static Option<WithStringPointerState<Token>> TokenizeStandaloneLetter(StringWithPointer strP) =>
        GetRestOfString(strP).Match(

                Some: restStr => Regex.Match(restStr,
                    "^([a-z] )|^( [a-z] )|^([a-z])$",
                    RegexOptions.IgnoreCase)
                    .IfMatchSuccessCreateTokenWithState(strP, TokenType.StandaloneLetter),

                None: new WithStringPointerState<Token>(
                    new Token("", TokenType.End), strP
                 )
            );

    //Match with strings starting with " 89x990", "0x99", " 2x9847 "
    public static Option<WithStringPointerState<Token>> TokenizeNumberxNumber(StringWithPointer strP) =>
        GetRestOfString(strP).Match(

                Some: restStr => Regex.Match(restStr,
                    "^([0-9]+x[0-9]+ )|^( [0-9]+x[0-9]+ )|^([0-9]+x[0-9]+)$",
                    RegexOptions.IgnoreCase)
                    .IfMatchSuccessCreateTokenWithState(strP, TokenType.NumberxNumber),

                None: new WithStringPointerState<Token>(
                    new Token("", TokenType.End), strP
                 )
            );


    private static Option<WithStringPointerState<Token>> IfMatchSuccessCreateTokenWithState(
                                                            this Match match,
                                                            StringWithPointer strP,
                                                            TokenType tokenType) =>
        match.Success?
            Some(new WithStringPointerState<Token>(
                    new Token(match.Value.Trim(), tokenType),
                    strP.IncrementBy(match.Value.Length)
                )
            ):
            None;
}
