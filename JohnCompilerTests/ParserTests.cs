using Microsoft.VisualStudio.TestTools.UnitTesting;
using JohnCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParserTest1()
        {
            var scanner = new Scanner("");
            var parser = new Parser(scanner);
            parser.Parse();

        }
        [TestMethod]
        public void ExpressionNumberTest()
        {
            var scanner = new Scanner("30 = 5 * 6 ] ] ");
            var parser = new Parser(scanner);
            var node = parser.Expression();
            Assert.IsNotNull(node);
        }
    }
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        public void ScannerTest1()
        {


            var scanner = new Scanner(file);
            var token = new Token();
            string output = "";
            while (token.value != "end")
            {
                token = scanner.Next();
                output += token.value + " ";
            }
            Assert.AreEqual(output, file);

        }
        private string file = "aa : bb : 4.5 end ";
        private string file2 = @"PROGRAM testy VAR
temp, tempy : INTEGER;
    test : ARRAY[1 : 10] OF REAL
FUNCTION testfunc(testarg : ARRAY[1 : 10] OF INTEGER) : ARRAY[1 : 10] OF REAL;
VAR
    temp2 : REAL;
BEGIN
    temp2 := 1;
END
PROCEDURE testproc;
VAR
    temp2 : REAL;
BEGIN
    temp2 := 1;
END
BEGIN
    temp := testfunc(2);
    testproc(4);
   { temp := testfunc2(4) * (3 + 4) + (4 + tempy);}
test[2] := - 3;
    temp := 5  6 + 2  4;
    temp := 1 + 5 <= 3 + 9;
    IF temp = 5 THEN
    temp := 6 ELSE
    temp := 1;
    WHILE temp = 2 DO
    temp := 4;
    READ(temp);
    WRITE(temp + 5);
END
.
}";
    }
}