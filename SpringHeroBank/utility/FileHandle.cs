using System;
using System.Collections.Generic;
using System.IO;
using SpringHeroBank.entity;
namespace SpringHeroBank.utility {
    public class FileHandle {
        public static List<Transaction> ReadTransaction (string filePath) {
            List<Transaction> list = new List<Transaction> ();
            string[] text = System.IO.File.ReadAllLines (filePath);           
            for (var i = 0; i < text.Length; i++) {
                if (i == 0) {
                    continue;
                }
                var lineSplitted = text[i].Split ("|", StringSplitOptions.RemoveEmptyEntries);
                if (lineSplitted.Length == 8) {
                    var transaction = new Transaction () {
                    Id = lineSplitted[0],
                    SenderAccountNumber = lineSplitted[1],
                    ReceiverAccountNumber = lineSplitted[2],
                    Type = (Transaction.TransactionType) Int32.Parse (lineSplitted[3]),
                    Amount = Decimal.Parse (lineSplitted[4]),
                    Content = lineSplitted[5],
                    CreatedAt = lineSplitted[6],
                    Status = (Transaction.ActiveStatus) Int32.Parse (lineSplitted[7])
                    };
                    list.Add (transaction);
                };
            }
            return list;
        }
        public static Dictionary<string, Account> ReadAccounts () {
            var dictionary = new Dictionary<string, Account> ();
            var lines = File.ReadAllLines ("ForgetMeNot.txt");
            for (var i = 0; i < lines.Length; i += 1) {
                if (i == 0) {
                    continue;
                }

                var linesSplited = lines[i].Split ("|");
                if (linesSplited.Length == 6) {
                    var acc = new Account () {
                    AccountNumber = linesSplited[0],
                    Username = linesSplited[1],
                    FullName = linesSplited[2],
                    Balance = Decimal.Parse (linesSplited[3]),
                    Salt = linesSplited[4],
                    Status = (Account.ActiveStatus) Int32.Parse (linesSplited[5])
                    };
                    if (dictionary.ContainsKey (acc.AccountNumber)) {
                        continue;
                    }

                    dictionary.Add (acc.AccountNumber, acc);
                }
            }
            List<Transaction> list = new List<Transaction> ();
            Dictionary<string, decimal> dictionaryTransaction = new Dictionary<string, decimal> ();
            var listTransactions = ReadTransaction("ForgetMeNot.txt");
            foreach (var transaction in list) {
                if (dictionaryTransaction.ContainsKey (transaction.SenderAccountNumber)) {
                    dictionaryTransaction[transaction.SenderAccountNumber] += transaction.Amount;
                } else {
                    dictionaryTransaction.Add (transaction.SenderAccountNumber, transaction.Amount);
                }
            }

            var accountDictionary = FileHandle.ReadAccounts ();
            foreach (var accountItem in accountDictionary) {
                if (dictionaryTransaction.ContainsKey (accountItem.Value.AccountNumber)) {
                    // Tạm thời coi số dư là tổng số tiền giao dịch.
                    accountItem.Value.Balance = dictionaryTransaction[accountItem.Value.AccountNumber];
                }
            }

            foreach (var account in accountDictionary.Values) {
                Console.WriteLine (account.Username + " - " + account.FullName + " - " + account.Salt + " - " + account.Balance);
            }
            return dictionary;
        }
    }

}