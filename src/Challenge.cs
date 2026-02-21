// DESAFIO: Regras de Desconto - SOLUÇÃO: Padrão Interpreter 

using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{
    public class ShoppingCart
    {
        public decimal TotalValue { get; set; }
        public int ItemQuantity { get; set; }
        public string CustomerCategory { get; set; }
        public bool IsFirstPurchase { get; set; }
    }

    public interface IExpression
    {
        decimal Evaluate(ShoppingCart cart);
    }

    public class QuantityGreaterThan : IExpression
    {
        private readonly int _threshold;
        public QuantityGreaterThan(int t) => _threshold = t;
        public decimal Evaluate(ShoppingCart c) => c.ItemQuantity > _threshold ? 1 : 0;
    }

    public class ValueGreaterThan : IExpression
    {
        private readonly decimal _threshold;
        public ValueGreaterThan(decimal t) => _threshold = t;
        public decimal Evaluate(ShoppingCart c) => c.TotalValue > _threshold ? 1 : 0;
    }

    public class CategoryEquals : IExpression
    {
        private readonly string _category;
        public CategoryEquals(string c) => _category = c;
        public decimal Evaluate(ShoppingCart c) => c.CustomerCategory == _category ? 1 : 0;
    }

    public class FirstPurchase : IExpression
    {
        public decimal Evaluate(ShoppingCart c) => c.IsFirstPurchase ? 1 : 0;
    }

    public class AndExpression : IExpression
    {
        private readonly IExpression _left, _right;
        public AndExpression(IExpression l, IExpression r) { _left = l; _right = r; }
        public decimal Evaluate(ShoppingCart c) => (_left.Evaluate(c) == 1 && _right.Evaluate(c) == 1) ? 1 : 0;
    }

    public class OrExpression : IExpression
    {
        private readonly IExpression _left, _right;
        public OrExpression(IExpression l, IExpression r) { _left = l; _right = r; }
        public decimal Evaluate(ShoppingCart c) => (_left.Evaluate(c) == 1 || _right.Evaluate(c) == 1) ? 1 : 0;
    }

    public class RuleExpression
    {
        private readonly IExpression _condition;
        private readonly decimal _discount;

        public RuleExpression(IExpression condition, decimal discount)
        {
            _condition = condition;
            _discount = discount;
        }

        public decimal Evaluate(ShoppingCart cart)
        {
            if (_condition.Evaluate(cart) == 1)
            {
                Console.WriteLine($"✓ Regra aplicada: {_discount}% desconto");
                return _discount;
            }
            return 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Regras de Desconto (Interpreter Pattern) ===\n");

            var rules = new List<RuleExpression>
            {
                new RuleExpression(new AndExpression(new QuantityGreaterThan(10), new ValueGreaterThan(1000)), 15),
                new RuleExpression(new CategoryEquals("VIP"), 20),
                new RuleExpression(new FirstPurchase(), 10)
            };

            var cart1 = new ShoppingCart { TotalValue = 1500m, ItemQuantity = 15, CustomerCategory = "Regular", IsFirstPurchase = false };
            var cart2 = new ShoppingCart { TotalValue = 500m, ItemQuantity = 5, CustomerCategory = "VIP", IsFirstPurchase = false };
            var cart3 = new ShoppingCart { TotalValue = 200m, ItemQuantity = 2, CustomerCategory = "Regular", IsFirstPurchase = true };

            foreach (var rule in rules) rule.Evaluate(cart1);
            foreach (var rule in rules) rule.Evaluate(cart2);
            foreach (var rule in rules) rule.Evaluate(cart3);

        }
    }
}
