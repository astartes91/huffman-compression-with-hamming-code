using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman
{
    //этот класс представляет символ как узел дерева с атрибутами вероятность, символ, код, и 2 потомка (левый и правый)
    public class Symbol
    {
        private Symbol rightChild;
        public Symbol RightChild
        {
            get
            {
                return rightChild;
            }
            set
            {
                this.rightChild = value;
            }
        }

        private Symbol leftChild;
        public Symbol LeftChild
        {
            get
            {
                return leftChild;
            }
            set
            {
                this.leftChild = value;
            }
        }

        private string symbols;
        public string Symbols
        {
            get
            {
                return symbols;
            }
            set
            {
                symbols = value;
            }
        }

        private string code;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        private double probability;
        public double Probability
        {
            get
            {
                return probability;
            }
            set
            {
                probability = value;
            }
        }

        public Symbol(string symbols, double probability) 
        {
            this.probability = probability;
            this.symbols = symbols;
        } 
    }
}