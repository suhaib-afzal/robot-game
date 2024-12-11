using LanguageExt;
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

namespace RobotApp.Parsing.Tokenization;


public static class TokenizerFunctions
{
    public static Doc<TokenLine> TokenizeDocument(string rawDoc)
    {
        throw new NotImplementedException();
    }

    public static Doc<TextLine> Chunker(string rawDoc)
    {
        var chunks = rawDoc.Split(Environment.NewLine)
            .Zip(Enumerable.Range(1, int.MaxValue))
            .Map(tup => new TextLine() { LineNumber = tup.Item2, Text = tup.Item1 })
            .Split(txtLine => txtLine.Text.Trim() == "")
            .Filter(chunk => chunk.Any())
            .Map(lines => new Chunk<TextLine>() { values = lines.ToList() })
            .ToList();

        var textDoc = new Doc<TextLine>(chunks);
        return textDoc;
    }

    public static Either<TokenizeFail, List<Token>> Tokenize(string str)
    {
        throw new NotImplementedException();
    }

    public static Option<WithStringPointerState<Token>> TokenizeWord(StringWithPointer strP)
    {
        //Match with strings starting with " kjkdokf", "jdciojcdij", " kcmcm " 
        var match = Regex.Match(strP.Value[strP.Pointer..], 
                               "^([a-z]{2,} )|^( [a-z]{2,} )|^([a-z]{2,})$", 
                               RegexOptions.IgnoreCase);
        if(match.Success)
        {
            var val = match.Value;
            var nextState = IncrementBy(strP, match.Value.Length);
            var token = new Token(val, TokenType.Word);
            return new WithStringPointerState<Token>(token, nextState);
        }
        else
        {
            return None;
        }
    }

    public static Option<WithStringPointerState<Token>> TokenizeNumber(StringWithPointer strP)
    {
        var match = Regex.Match(strP.Value[strP.Pointer..],
                                "^([0-9]{1,} )|^( [0-9]{1,} )|^([0-9]{1,})$");
        if (match.Success)
        {
            var val = match.Value;
            var nextState = IncrementBy(strP, match.Value.Length);
            var token = new Token(val, TokenType.Number);
            return new WithStringPointerState<Token>(token, nextState);
        }
        else
        {
            return None;
        }
    }

    public static Option<WithStringPointerState<Token>> TokenizeStandaloneLetter(StringWithPointer strP)
    {
        var match = Regex.Match(strP.Value,
                               "^([a-z] )|^( [a-z] )|^([a-z])$",
                               RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var val = match.Value;
            var nextState = IncrementBy(strP, match.Value.Length);
            var token = new Token(val, TokenType.Number);
            return new WithStringPointerState<Token>(token, nextState);
        }
        else
        {
            return None;
        }
    }


    public static Either<TokenizeFail, List<T>> Many<T>(Func<StringWithPointer, Either<TokenizeFail, T>> tokenizer)
    {
        throw new NotImplementedException();
    }

}
