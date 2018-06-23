﻿using System;
using SpringHeroBank.entity;
using SpringHeroBank.model;
using SpringHeroBank.utility;

namespace SpringHeroBank.controller {
    public class AccountController {
        private AccountModel model = new AccountModel ();

        public void Register () {
            Console.WriteLine ("Please enter account information");
            Console.WriteLine ("-----------------------------------");
            Console.WriteLine ("Username: ");
            var username = Console.ReadLine ();
            Console.WriteLine ("Password: ");
            var password = Console.ReadLine ();
            Console.WriteLine ("Confirm Password: ");
            var cpassword = Console.ReadLine ();
            Console.WriteLine ("Identity Card: ");
            var identityCard = Console.ReadLine ();
            Console.WriteLine ("Full Name: ");
            var fullName = Console.ReadLine ();
            Console.WriteLine ("Email: ");
            var email = Console.ReadLine ();
            Console.WriteLine ("Phone: ");
            var phone = Console.ReadLine ();
            var account = new Account (username, password, cpassword, identityCard, phone, email, fullName);
            var errors = account.CheckValid ();
            var checkAcc = model.GetAccountByUserName (username);
            if (checkAcc == null && errors.Count == 0) {
                model.Save (account);
                Console.WriteLine ("Register success!");
                System.Console.WriteLine("Press enter to continue!");
                Console.ReadLine ();
            } else if(checkAcc != null) {
                System.Console.WriteLine("Account username already existed!");
            } else {
                Console.Error.WriteLine ("Please fix following errors and try again.");
                foreach (var messagErrorsValue in errors.Values) {
                    Console.Error.WriteLine (messagErrorsValue);
                }

                Console.ReadLine ();
            }
        }

        public Boolean DoLogin () {
            // Lấy thông tin đăng nhập phía người dùng.
            Console.WriteLine ("Please enter account information");
            Console.WriteLine ("-----------------------------------");
            Console.WriteLine ("Username: ");
            var username = Console.ReadLine ();
            Console.WriteLine ("Password: ");
            var password = Console.ReadLine ();
            var account = new Account (username, password);
            // Tiến hành validate thông tin đăng nhập. Kiểm tra username, password khác null và length lớn hơn 0.
            var errors = account.ValidLoginInformation ();
            if (errors.Count > 0) {
                Console.WriteLine ("Invalid login information. Please fix errors below.");
                foreach (var messagErrorsValue in errors.Values) {
                    Console.Error.WriteLine (messagErrorsValue);
                }

                Console.ReadLine ();
                return false;
            }

            account = model.GetAccountByUserName (username);
            if (account == null) {
                // Sai thông tin username, trả về thông báo lỗi không cụ thể.
                Console.WriteLine ("Invalid login information. Please try again.");
                return false;
            }

            // Băm password người dùng nhập vào kèm muối và so sánh với password đã mã hoá ở trong database.
            if (account.Password != Hash.GenerateSaltedSHA1 (password, account.Salt)) {
                // Sai thông tin password, trả về thông báo lỗi không cụ thể.
                Console.WriteLine ("Invalid login information. Please try again.");
                return false;
            }

            // Login thành công. Lưu thông tin đăng nhập ra biến static trong lớp Program.
            Program.currentLoggedIn = account;
            return true;
        }

        public void Withdraw () {
            Console.WriteLine ("Withdraw.");
            Console.WriteLine ("---------------------------------");
            Console.WriteLine ("Please enter amount to withdraw: ");
            var amount = Utility.GetUnsignDecimalNumber ();
            Console.WriteLine ("Please enter message content: ");
            var content = Console.ReadLine ();
            //            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
            var historyTransaction = new Transaction {
                Id = Guid.NewGuid ().ToString (),
                Type = Transaction.TransactionType.WITHDRAW,
                Amount = amount,
                Content = content,
                SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                ReceiverAccountNumber = Program.currentLoggedIn.AccountNumber,
                Status = Transaction.ActiveStatus.DONE
            };
            if (model.UpdateBalance (Program.currentLoggedIn, historyTransaction)) {
                Console.WriteLine ("Transaction success!");
            } else {
                Console.WriteLine ("Transaction fails, please try again!");
            }
            Program.currentLoggedIn = model.GetAccountByUserName (Program.currentLoggedIn.Username);
            Console.WriteLine ("Current balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine ("Press enter to continue!");
            Console.ReadLine ();
        }

        public void Deposit () {
            Console.WriteLine ("Deposit.");
            Console.WriteLine ("---------------------------------");
            Console.WriteLine ("Please enter amount to deposit: ");
            var amount = Utility.GetUnsignDecimalNumber ();
            Console.WriteLine ("Please enter message content: ");
            var content = Console.ReadLine ();
            //            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
            var historyTransaction = new Transaction {
                Id = Guid.NewGuid ().ToString (),
                Type = Transaction.TransactionType.DEPOSIT,
                Amount = amount,
                Content = content,
                SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                ReceiverAccountNumber = Program.currentLoggedIn.AccountNumber,
                Status = Transaction.ActiveStatus.DONE
            };
            if (model.UpdateBalance (Program.currentLoggedIn, historyTransaction)) {
                Console.WriteLine ("Transaction success!");
            } else {
                Console.WriteLine ("Transaction fails, please try again!");
            }
            Program.currentLoggedIn = model.GetAccountByUserName (Program.currentLoggedIn.Username);
            Console.WriteLine ("Current balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine ("Press enter to continue!");
            Console.ReadLine ();
        }

        public void Transfer () {
            Console.WriteLine ("Tranfer.");
            Console.WriteLine ("---------------------------------");
            Console.WriteLine ("Please enter receiver bank account ");
            var receiverBankAccountNumber = Console.ReadLine ();
            var receiverAccount = model.GetAccountByAccountNumber (receiverBankAccountNumber);
            //Console.WriteLine("Receiver's Fullname : " + receiverAccount.FullName);
            Console.WriteLine ("Please enter amount to tranfer: ");
            var amount = Utility.GetUnsignDecimalNumber ();
            if (Program.currentLoggedIn.Balance < amount) {
                System.Console.WriteLine ("Not enough money to tranfer");
                Console.WriteLine ("Press enter to continue!");
                Console.ReadLine ();
            } else {
                Console.WriteLine ("Please enter message content: ");
                var content = Console.ReadLine ();
                //            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
                Console.WriteLine ("You are transfering " + amount + " to " + receiverAccount.FullName);
                Console.WriteLine ("Are you sure you want to tranfer? (Y|N)");
                var choice = Console.ReadLine ();
                if (choice.Equals ("Y")) {
                    var historyTransaction = new Transaction {
                        Id = Guid.NewGuid ().ToString (),
                        Type = Transaction.TransactionType.TRANSFER,
                        Amount = amount,
                        Content = content,
                        SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                        ReceiverAccountNumber = receiverBankAccountNumber,
                        Status = Transaction.ActiveStatus.DONE
                    };
                    if (model.Tranfer (Program.currentLoggedIn, receiverAccount, historyTransaction)) {
                        Console.WriteLine ("Transaction success!");
                    } else {
                        Console.WriteLine ("Transaction fails, please try again!");
                    }
                    Program.currentLoggedIn = model.GetAccountByUserName (Program.currentLoggedIn.Username);
                    Console.WriteLine ("Current balance: " + Program.currentLoggedIn.Balance);
                    Console.WriteLine ("Press enter to continue!");
                    Console.ReadLine ();
                } else {
                    Console.WriteLine ("Tranfer failed!");
                    Console.WriteLine ("Press enter to continue!");
                    Console.ReadLine ();
                }
            }
        }

        public void CheckBalance () // Dịch bởi Phúc.
        {
            Program.currentLoggedIn = model.GetAccountByUserName (Program.currentLoggedIn.Username);
            Console.WriteLine ("Account Information");
            Console.WriteLine ("---------------------------------");
            Console.WriteLine ("Full name: " + Program.currentLoggedIn.FullName);
            Console.WriteLine ("Account number: " + Program.currentLoggedIn.AccountNumber);
            Console.WriteLine ("Balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine ("Press enter to continue!");
            Console.ReadLine ();
        }
    }
}