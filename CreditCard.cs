using System;

namespace ConsoleApp10
{
    public class CreditCard
    {
        // fields / properties
        public string CardNumber { get; private set; }
        public string OwnerName { get; private set; }
        public DateTime Expiry { get; private set; }
        private string _pin;
        public double CreditLimit { get; private set; } // how much credit available
        public double Balance { get; private set; } // positive means money in account, negative could means debt

        // goal amount for event
        private double _goalAmount = double.PositiveInfinity;

        // events
        public event Action<string> OnDeposit;
        public event Action<string> OnWithdraw;
        public event Action<double> OnCreditStart;
        public event Action<string> OnGoalReached;
        public event Action<string> OnPINChanged;

        // predicate to check pin
        private Predicate<string> IsCorrectPIN => (p) => p == _pin;

        // ctor
        public CreditCard(string num, string owner, DateTime expiry, string pin, double creditLimit, double initialMoney)
        {
            CardNumber = num;
            OwnerName = owner;
            Expiry = expiry;
            _pin = pin;
            CreditLimit = creditLimit;
            Balance = initialMoney;
        }

        // method to deposit money
        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                OnDeposit?.Invoke("Deposit failed: amount must be > 0");
                return;
            }

            Balance += amount;
            OnDeposit?.Invoke($"Deposited {amount}. New balance: {Balance}");

            // check goal
            if (Balance >= _goalAmount)
            {
                OnGoalReached?.Invoke($"Goal reached! Balance is {Balance} (goal was {_goalAmount})");
                // reset goal so it don't spam
                _goalAmount = double.PositiveInfinity;
            }
        }

        // method to withdraw money
        public bool Withdraw(double amount, string pin)
        {
            if (amount <= 0)
            {
                OnWithdraw?.Invoke("Withdraw failed: amount must be > 0");
                return false;
            }

            if (!IsCorrectPIN(pin))
            {
                OnWithdraw?.Invoke("Withdraw failed: wrong PIN");
                return false;
            }

            // if enough balance -> normal withdraw
            if (Balance >= amount)
            {
                Balance -= amount;
                OnWithdraw?.Invoke($"Withdrawn {amount}. New balance: {Balance}");
                return true;
            }

            // not enough balance -> try use credit (if available)
            double need = amount - Balance;
            if (need <= CreditLimit)
            {
                // use all balance and use credit for rest
                double usingCredit = need;
                Balance = 0;
                CreditLimit -= usingCredit;
                OnCreditStart?.Invoke(usingCredit);
                OnWithdraw?.Invoke($"Withdrawn {amount} by using credit {usingCredit}. Balance: {Balance}, Remaining credit limit: {CreditLimit}");
                return true;
            }
            else
            {
                OnWithdraw?.Invoke($"Withdraw failed: not enough money and credit. Need {need} but credit limit {CreditLimit}");
                return false;
            }
        }

        // start using credit explicitly
        public bool StartUsingCredit(double amount)
        {
            if (amount <= 0) return false;
            if (amount <= CreditLimit)
            {
                CreditLimit -= amount;
                OnCreditStart?.Invoke(amount);
                return true;
            }
            else
            {
                return false;
            }
        }

        // change pin
        public bool ChangePIN(string oldPin, string newPin)
        {
            if (!IsCorrectPIN(oldPin))
            {
                OnPINChanged?.Invoke("PIN change failed: old PIN is wrong");
                return false;
            }

            _pin = newPin;
            OnPINChanged?.Invoke("PIN changed success");
            return true;
        }

        // set a goal for reaching amount
        public void SetGoalAmount(double goal)
        {
            if (goal <= 0) return;
            _goalAmount = goal;
            // maybe check immediately
            if (Balance >= _goalAmount)
            {
                OnGoalReached?.Invoke($"Goal already reached at set time: balance {Balance}");
                _goalAmount = double.PositiveInfinity;
            }
        }

        // For debug / display
        public override string ToString()
        {
            return $"Card {CardNumber} owner {OwnerName}, exp {Expiry:d}, bal {Balance}, credit limit {CreditLimit}";
        }
    }
}