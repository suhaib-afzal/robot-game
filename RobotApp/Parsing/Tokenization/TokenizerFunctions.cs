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

    public static Either<TokenizeFail, WithStringPointerState<Token>> TokenizeWord(StringWithPointer strP)
    {
        throw new NotImplementedException();
    }

    public static Either<TokenizeFail, WithStringPointerState<Token>> TokenizeNumber(StringWithPointer strP)
    {
        throw new NotImplementedException();
    }

    public static Either<TokenizeFail, Option<WithStringPointerState<Token>>> TokenizeStandaloneLetter(StringWithPointer strP)
    {
        
        var thinhg = from alpha in OptionT(ConsumeChar(strP, _ => true, "Failed to consume any character"))
                     from spc in ConsumeChar(alpha.UpdatedStringWPointer, c => c == ' ', "Expecting space after Alphabetic Character")
                     select spc;

    }

    public static Either<TokenizeFail, Option<WithStringPointerState<char>>> ConsumeChar(StringWithPointer strP, Func<char, bool> pred, string failedMessage)
    {
        return GetCharAndIncrement(strP)
            .Match<Either<TokenizeFail, Option<WithStringPointerState<char>>>>(
              None: () => Option<WithStringPointerState<char>>.None,
              Some: tup =>
              {
                  var c = tup.Item1;
                  var state = tup.Item2;
                  if (pred(c))
                  {
                      return Some(new WithStringPointerState<char>(c, state));
                  }

                  return new TokenizeFail(failedMessage);
              }
            );
    }

    public static Either<TokenizeFail, List<T>> Many<T>(Func<StringWithPointer, Either<TokenizeFail, T>> tokenizer)
    {
        throw new NotImplementedException();
    }

}
