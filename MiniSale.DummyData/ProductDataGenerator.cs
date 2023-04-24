using MiniSale.DummyData.Models;
using System;
using System.Text;

namespace MiniSale.DummyData
{
    public class ProductDataGenerator
    {
        private readonly Random _random = new Random();
        private const string _barCodeSimbols= "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const string _nameSombols= "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly int _count;
        private readonly int _priceMin;
        private readonly int _priceMax;
        private readonly int _pluMin;
        private readonly int _pluMax;

        private HashSet<string> _barCodeList;
        private int _numNullBarcodes;

        private HashSet<int> _pluNumbers;

        public ProductDataGenerator(int count, int priceMin, int priceMax, int pluMin, int pluMax)
        {
            _count = count;
            _priceMin = priceMin;
            _priceMax = priceMax;
            _pluMin = pluMin;
            _pluMax = pluMax;
        }

        public List<ProductDummyModel> GenerateDummyData()
        {
            var res = new List<ProductDummyModel>();

            GenerateRandomBarCodes();
            GenerateRandomPLUNumbers();

            for (int i = 0; i < _count; i++)
            {
                var p = new ProductDummyModel()
                {
                    Id = Guid.NewGuid(),
                    Name = GenerateProductName(),
                    Price = GenerateRandomPrice(),
                    BarCode = GetNextRandomBarCode(_count - i + 1),
                    PLU = GetNextGeneratedPLU()
                };
                res.Add(p);
            }
            return res;
        }

        private string GenerateProductName()
        {
            int length = _random.Next(5, 16);
            return GenerateRandomString(length);
        }

        string GenerateRandomString(int length)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
                sb.Append(_nameSombols[_random.Next(_nameSombols.Length)]);

            return sb.ToString();
        }

        int GenerateRandomPrice()
        {
            int range = _priceMax - _priceMin + 1;
            int randomNumber = _random.Next(range) + _priceMin;
            return (int)Math.Round(randomNumber / 10.0) * 10;
        }

        /// <summary>
        /// Return randomly null or one of a barcodes
        /// </summary>
        /// <returns></returns>
        private string GetNextRandomBarCode(int remainsProductsToApply) //To Do: Make not related from count
        {
            if ((_numNullBarcodes > 0 && _random.Next(2) == 0) || _numNullBarcodes == remainsProductsToApply)
            {
                _numNullBarcodes--;
                return null;
            }

            if (!_barCodeList.Any()) //To Do: Generate specific Exception
                return null;

            var res = _barCodeList.FirstOrDefault();
            _barCodeList.Remove(res);
            return res;
        }

        string GenerateRandomBarcode()
        {
            char[] barcodeChars = new char[13];

            for (int i = 0; i < barcodeChars.Length; i++)
                barcodeChars[i] = _barCodeSimbols[_random.Next(_barCodeSimbols.Length)];

            return new string(barcodeChars);
        }

        private void GenerateRandomBarCodes()
        {
            double nullProbability = 0.1;
            _numNullBarcodes = (int)Math.Round(_count * nullProbability);
            int numNonNullBarcodes = _count - _numNullBarcodes;

            _barCodeList = new HashSet<string>();
            for (int i = 0; i < numNonNullBarcodes; i++)
            {
                string barcode = GenerateRandomBarcode();
                while (_barCodeList.Contains(barcode))
                {
                    barcode = GenerateRandomBarcode();
                }
                _barCodeList.Add(barcode);
            }
        }

        int GetNextGeneratedPLU()
        {
            var res = _pluNumbers.FirstOrDefault();
            _pluNumbers.Remove(res);
            return res;
        }

        int GenerateRandomPLUNumber()
        {
            int randomNumber = _random.Next(_pluMin, _pluMax + 1);
            return randomNumber;
        }

        void GenerateRandomPLUNumbers()
        {
            _pluNumbers = new HashSet<int>();

            for (int i = 0; i < _count; i++)
            {
                int number = GenerateRandomPLUNumber();
                while (_pluNumbers.Contains(number))
                {
                    number = GenerateRandomPLUNumber();
                }
                _pluNumbers.Add(number);
            }
        }
    }
}