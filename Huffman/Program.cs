using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Huffman
{
    public class Program
    {
        private const int constSize = 32;
        private static ArrayList newSymbolsArray = new ArrayList();
        private static IComparer compareProbabilities = new CompareProbabilities();
        private static ArrayList symbolsArray = new ArrayList();
        private static int lastStringSize;
        private static bool flag = false;
 
        public class CompareProbabilities : IComparer
        {
            //сравнение по вероятностям
            int IComparer.Compare(object o1, object o2)
            {
                Symbol s1 = (Symbol)o1;
                Symbol s2 = (Symbol)o2;
                
                if (s1.Probability < s2.Probability) return -1;
                else if (s1.Probability > s2.Probability) return 1;
                else return 0;
            }
        }
        //создание  дерева
        private static void CreateTree()
        {
            int size = 32;

            while (size > 1)
            {
                //присваивание одной ветке кода "0", другой "1"
                ((Symbol)symbolsArray[0]).Code = "1";
                ((Symbol)symbolsArray[1]).Code = "0";
                //создание псевдовершины (родителя) для двух симолов с суммарной верояностью
                Symbol parent = new Symbol(((Symbol)symbolsArray[0]).Symbols + ((Symbol)symbolsArray[1]).Symbols,
                                            ((Symbol)symbolsArray[0]).Probability + ((Symbol)symbolsArray[1]).Probability);
                //левый и правый потомки псевдовершины
                parent.RightChild = ((Symbol)symbolsArray[0]);
                parent.LeftChild = ((Symbol)symbolsArray[1]);
                //удаление потомков из списка свободных узлов
                symbolsArray.RemoveAt(0);
                symbolsArray.RemoveAt(0);
                //добавление псевдовершины в массив свободных узлов
                symbolsArray.Add(parent);
                symbolsArray.Sort(compareProbabilities);

                size--;
            }
        }
        //рекурсивнй обход дерева
        private static void TreeTraversal(Symbol sym, string ccode)
        {
            string code = ccode;
            //если узел не имеет потомков, значит, это лист (символ)
            if (sym.LeftChild == null && sym.RightChild == null)
            {
                sym.Code = code;
                newSymbolsArray.Add(sym);
            }
            //иначе, рекурсивно вызвать метод TreeTraversal для левого и правого потомка
            else 
            {
                if(sym.LeftChild != null)
                {
                    code += sym.LeftChild.Code;
                    TreeTraversal(sym.LeftChild, code);
                    code = code.Remove(code.Length - 1, 1);
                }
                if (sym.RightChild != null)
                {
                    code += sym.RightChild.Code;
                    TreeTraversal(sym.RightChild, code);
                    code = code.Remove(code.Length - 1, 1);
                }
            }
        }
        //кодирование текста методом Хаффмана
        private static string HuffmanEncoding(ref string text) 
        {
            string encodedText = "";

            for (int i = 0; i < text.Length; i++)
            {
                int j;
                Console.Write(text[i] + "   ");
                for (j = 0; j < newSymbolsArray.Count; j++)
                {

                    if (text[i] == ' ' && ((Symbol)newSymbolsArray[j]).Symbols.Equals("пробел"))
                    {
                        Console.WriteLine(((Symbol)newSymbolsArray[j]).Code);
                        encodedText += ((Symbol)newSymbolsArray[j]).Code;
                        break;
                    }
                    else if ((text[i] == 'е' || text[i] == 'ё') && ((Symbol)newSymbolsArray[j]).Symbols.Equals("е,ё"))
                    {
                        Console.WriteLine(((Symbol)newSymbolsArray[j]).Code);
                        encodedText += ((Symbol)newSymbolsArray[j]).Code;
                        break;
                    }
                    else if ((text[i] == 'ъ' || text[i] == 'ь') && ((Symbol)newSymbolsArray[j]).Symbols.Equals("ъ,ь"))
                    {
                        Console.WriteLine(((Symbol)newSymbolsArray[j]).Code);
                        encodedText += ((Symbol)newSymbolsArray[j]).Code;
                        break;
                    }
                    else if (((Symbol)newSymbolsArray[j]).Symbols[0] == text[i])
                    {
                        Console.WriteLine(((Symbol)newSymbolsArray[j]).Code);
                        encodedText += ((Symbol)newSymbolsArray[j]).Code;
                        break;
                    }
                }
                if (j == 32) Console.WriteLine();
            }

            return encodedText;
        }
        //декодирование текста
        private static void HuffmanDecoding(ref string encodedText) 
        {
            string code = encodedText[0].ToString();
            
            for (int i = 0; i < encodedText.Length; i++)
            {
                int j;
                for (j = 0; j < newSymbolsArray.Count; j++)
                {
                    if (code.Equals("0101"))
                    {
                        Console.Write(code + "   ");
                        Console.WriteLine("e");
                        if (i < encodedText.Length - 1) code = encodedText[i + 1].ToString();
                        break;
                    }
                    else if (code.Equals("101111"))
                    {
                        Console.Write(code + "   ");
                        Console.WriteLine("ь");
                        if (i < encodedText.Length - 1) code = encodedText[i + 1].ToString();
                        break;
                    }
                    else if (code.Equals("011"))
                    {
                        Console.Write(code + "   ");
                        Console.WriteLine(" ");
                        if (i < encodedText.Length - 1) code = encodedText[i + 1].ToString();
                        break;
                    }
                    else if (((Symbol)newSymbolsArray[j]).Code.Equals(code))
                    {
                        Console.Write(code + "   ");
                        Console.WriteLine(((Symbol)newSymbolsArray[j]).Symbols);
                        if (i < encodedText.Length - 1) code = encodedText[i + 1].ToString();
                        break;
                    }
                }
                if (j == 32 && i < encodedText.Length - 1) code += encodedText[i + 1].ToString();
            }
        }
        //добавление избыточности, используя код Хемминга
        private static string[] HammingEncoding(string text) 
        {
            int messageLength = text.Length;
            int quotient = messageLength / 4;
            if (messageLength % 4 != 0) flag = true;

            string[] strAr;
            if(flag) strAr = new string[quotient+1];
            else strAr = new string[quotient];

            for (int i = 0; i < quotient; i++)
            {
                string new_message = text.Substring(0, 4);
                strAr[i] = new_message;

                text = text.Remove(0, 4);
            }

            if (flag) 
            {
                lastStringSize = text.Length;
                strAr[quotient] = text;
                strAr[quotient] = strAr[quotient].PadRight(4, '0');
                quotient++;
            }

            for (int i = 0; i < quotient; i++)
            {
                string block = strAr[i];
                //вычисление контрольных разрядов 
                int a1 = int.Parse(block[3].ToString()) ^ int.Parse(block[2].ToString()) ^ int.Parse(block[0].ToString());
                int a2 = int.Parse(block[3].ToString()) ^ int.Parse(block[1].ToString()) ^ int.Parse(block[0].ToString());
                int a4 = int.Parse(block[2].ToString()) ^ int.Parse(block[1].ToString()) ^ int.Parse(block[0].ToString());

                block = block.Insert(3, a4.ToString());
                block = block.PadRight(6, a2.ToString().ToCharArray()[0]);
                block = block.PadRight(7, a1.ToString().ToCharArray()[0]);
                strAr[i] = block;

                Console.Write(strAr[i] + " ");
            }

            return strAr;
        }
        //преобразование кода Хемминга в коды Хаффмана
        private static string HammingDecoding(ref string[] redundantCodes) 
        {
            string str = "", str2 = "";
            for (int i = 0; i < redundantCodes.Length; i++) 
            {
                string redundantBlock = redundantCodes[i];
                //проверка на ошибки
label:          int[] sindrom = CheckOnSingleError(ref redundantBlock);
                //если синдром не нулевой, значит принят ошибочный блок
                if (sindrom[0] != 0 || sindrom[1] != 0 || sindrom[2] != 0) 
                {
                    //вычисление номера ошибочного разряда
                    int position = sindrom[0] * 4 + sindrom[1] * 2 + sindrom[2] * 1;

                    Console.WriteLine("Detecting error in {0} block, bit {1}, correction...", i+1, position);
                    
                    int bitWithError = int.Parse(redundantBlock[7 - position].ToString());
                    int correctedBit;
                    if(bitWithError == 0) correctedBit = 1;
                    else correctedBit = 0;
                    //исправление ошибки
                    redundantBlock = redundantBlock.Remove(7 - position, 1);
                    redundantBlock = redundantBlock.Insert(7 - position, correctedBit.ToString());

                    goto label;
                }

                str2 += redundantBlock; 
                //преобразование в код Хаффмана
                redundantBlock = redundantBlock.Remove(5, 2);
                redundantBlock = redundantBlock.Remove(3, 1);

                if (flag && i == redundantCodes.Length - 1) redundantBlock = redundantBlock.Remove(lastStringSize, 4 - lastStringSize);

                str += redundantBlock;
                str2 += " ";
            }

            Console.WriteLine("\nCorrected codes");
            Console.WriteLine(str2);

            return str;
        }
        //проверка на ошибку
        private static int[] CheckOnSingleError(ref string redundantBlock) 
        {
            //вычисление синдрома
            int s1 = int.Parse(redundantBlock[3].ToString()) ^ int.Parse(redundantBlock[2].ToString())
                         ^ int.Parse(redundantBlock[1].ToString()) ^ int.Parse(redundantBlock[0].ToString());

            int s2 = int.Parse(redundantBlock[5].ToString()) ^ int.Parse(redundantBlock[4].ToString())
                    ^ int.Parse(redundantBlock[1].ToString()) ^ int.Parse(redundantBlock[0].ToString());

            int s3 = int.Parse(redundantBlock[6].ToString()) ^ int.Parse(redundantBlock[4].ToString())
                    ^ int.Parse(redundantBlock[2].ToString()) ^ int.Parse(redundantBlock[0].ToString());

            int[] sindrom = {s1, s2, s3};
            return sindrom;
        } 
        //генерация случайных ошибок
        private static void GenerateErrors(ref string[] redundantCodes)
        {
            string str = "";
            Random rand = new Random();

            for (int i = 0; i < redundantCodes.Length; i++)
            {
                string block = redundantCodes[i];
                int position = rand.Next(1, 8);
                
                int bit = int.Parse(block[7-position].ToString());
                int error;
                
                block = block.Remove(7-position, 1);
                
                if (bit == 1) error = 0;
                else error = 1;
                
                block = block.Insert(7 - position, error.ToString());
                redundantCodes[i] = block;

                Console.WriteLine("Generate error in {0} block, {1} bit", i+1, position);

                str += redundantCodes[i];
                str += " ";
            }

            Console.WriteLine("\nCodes with errors:");
            Console.WriteLine(str);
        }

        static void Main(string[] args)
        {            
            double[] probabilities = {
                                        0.143, 0.096, 0.09, 0.074, 0.064, 0.064, 0.056, 0.056, 0.047, 0.041, 0.04, 0.039, 0.036, 0.029, 0.026, 
                                         0.026, 0.024, 0.021, 0.02, 0.019, 0.016, 0.015, 0.015, 0.015, 0.014, 0.013, 0.01, 0.008, 0.007, 0.006, 0.003, 0.003 
                                     };

            string[] symbols = {
                                   "пробел", "о", "х", "е,ё", "а", "и", "н", "т", "с", "р", "ц", "в", "л", "к", "д", "м", "п", "у", "ф", "я", "ы", "б",
                                   "з", "ъ,ь", "г", "ч", "й", "ж", "ю", "ш", "щ", "э"
                               };

            
            //создание экземляров Symbol с заданной вероятностью и симоволом
            for (int i = 0; i < constSize; i++) 
            {
                symbolsArray.Add(new Symbol(symbols[i], probabilities[i]));
            }
            
            //сортировка массива по возрастанию вероятности
            symbolsArray.Sort(compareProbabilities);

            CreateTree(); 
            
            //обход дерева
            TreeTraversal((Symbol)symbolsArray[0], "");

            newSymbolsArray.Sort(compareProbabilities);
            Console.WriteLine("Codes of Russian alphabet:\n\n");
            for (int i = 0; i < constSize; i++) 
            {
                Console.WriteLine(((Symbol)newSymbolsArray[i]).Symbols + "   " + ((Symbol)newSymbolsArray[i]).Probability + "   "
                                + ((Symbol)newSymbolsArray[i]).Code + "\n");
            }

            Console.WriteLine("Please enter your text:\n");
            string text = Console.ReadLine();
            text = text.ToLower();

            Console.WriteLine("\n\nEncoded text:");
            //кодирование текста методом Хаффмана
            string encodedText = HuffmanEncoding(ref text);
            Console.WriteLine("\n\nEncoded text in string:");
            Console.WriteLine(encodedText);
            
            Console.WriteLine("\n\nEncoded text with redundancy (Hamming code):");
            //добавление избыточности
            string[] redundantCodes = HammingEncoding(encodedText);
            
            Console.WriteLine("\n\nGenerate errors:");
            //генерирование ошибок
            GenerateErrors(ref redundantCodes);

            Console.WriteLine("\nDecoding and correction:");
            //проверка блоков на одиночные ошибки и их исправление
            string encodedText2 = HammingDecoding(ref redundantCodes);
            
            Console.WriteLine("\n\nHuffman codes:");
            Console.WriteLine(encodedText2);

            Console.WriteLine("\n\nDecoded text:");
            //Декодирование текста
            HuffmanDecoding(ref encodedText2);
            
            Console.WriteLine();
        }
    }
}