using System;
using System.Collections.Generic;
using SpringHeroBank.entity;
using SpringHeroBank.model;
using SpringHeroBank.view;

namespace SpringHeroBank.controller {
    public class TransactionController {
        TransactionModel transactionModel = new TransactionModel ();
        List<Transaction> list = null;
        public void GetTransactionByTime () {

            System.Console.WriteLine ("Transaction information");
            System.Console.WriteLine ("------------------------------");
            System.Console.WriteLine ("1.Transactions in past week");
            System.Console.WriteLine ("2.Transactions in past month");
            System.Console.WriteLine ("3.Transactions in past year");
            System.Console.WriteLine ("4.All Transaction");
            System.Console.WriteLine ("5.Transactions in a single elective day");
            System.Console.WriteLine ("6.Transactions in custom range");
            System.Console.WriteLine ("7.Return");
            System.Console.WriteLine ("Please enter choice (1|2|3|4|5|6)");
            var a = Console.ReadLine ();
            var choice = Int32.Parse (a);

            switch (choice) {
                case 1:
                    list = transactionModel.GetTransactionsBySetTime ("WEEK", Program.currentLoggedIn.AccountNumber);
                    if(list.Count != 0){
                    SendInformationBack (list);
                    PrintTransaction (list,"pastWeek");
                    } else {System.Console.WriteLine( "Invalid transaction at that time");}
                    break;
                case 2:
                    list = transactionModel.GetTransactionsBySetTime ("MONTH", Program.currentLoggedIn.AccountNumber);
                    if(list.Count != 0){
                        SendInformationBack (list);
                        PrintTransaction (list,"pastMonth");
                    } else {System.Console.WriteLine( "Invalid transaction at that time");}
                    break;
                case 3:
                    list = transactionModel.GetTransactionsBySetTime ("YEAR", Program.currentLoggedIn.AccountNumber);
                    if(list.Count != 0){
                        SendInformationBack (list);
                    PrintTransaction (list,"pastYear");
                    } else {System.Console.WriteLine( "Invalid transaction at that time");} 
                    
                    break;
                case 4:
                    list = transactionModel.GetTransactionsByAccount (Program.currentLoggedIn.AccountNumber);
                    if(list.Count != 0) {
                        SendInformationBack (list);                   
                        PrintTransaction (list,"all");
                    } else {System.Console.WriteLine( "Invalid transaction at that time");}     
                    
                    break;
                case 5:
                    System.Console.WriteLine ("Please enter date you want to check ( Example : 18-05-2018 )");
                    System.Console.WriteLine ("Day : ");
                    int day = Int32.Parse (Console.ReadLine ());
                    System.Console.WriteLine ("Month : ");
                    int month = Int32.Parse (Console.ReadLine ());
                    System.Console.WriteLine ("Year : ");
                    int year = Int32.Parse (Console.ReadLine ());
                    var dateP = year+""+month+""+day;
                    if (ValidateDate (day, month, year)) {
                        var date = year + "-" + month + "-" + day;
                        list = transactionModel.GetTransactionsByTime (date, date, Program.currentLoggedIn.AccountNumber);
                        if(list.Count != 0 ){
                            SendInformationBack (list);
                            PrintTransaction (list,dateP);
                        }  else {System.Console.WriteLine( "Invalid transaction at that time");}                     
                    }
                    break;
                case 6:
                    System.Console.WriteLine ("Please enter custom range you want to check ( Example : 18-05-2018 )");
                    System.Console.WriteLine ("**From");
                    System.Console.WriteLine ("Day : ");
                    var fromDay = Int32.Parse (Console.ReadLine ());
                    System.Console.WriteLine ("Month : ");
                    var fromMonth = Int32.Parse (Console.ReadLine ());
                    System.Console.WriteLine ("Year : ");
                    var fromYear = Int32.Parse (Console.ReadLine ());
                    System.Console.WriteLine ("-------------");
                    System.Console.WriteLine ("**To");
                    System.Console.WriteLine ("Day : ");
                    var toDay = Int32.Parse (Console.ReadLine ());
                    System.Console.WriteLine ("Month : ");
                    var toMonth = Int32.Parse (Console.ReadLine ());
                    System.Console.WriteLine ("Year : ");
                    var toYear = Int32.Parse (Console.ReadLine ());
                    var dateP2 = toYear+""+toMonth+""+toDay+"-"+fromYear+""+fromMonth+""+fromDay;
                    if (ValidateDate (toDay, toMonth, toYear) && ValidateDate (fromDay, fromMonth, fromYear)) {
                        var to = toYear + "-" + toMonth + "-" + toDay;
                        var from = fromYear + "-" + fromMonth + "-" + fromDay;
                        list = transactionModel.GetTransactionsByTime (from, to, Program.currentLoggedIn.AccountNumber);
                        if(list.Count != 0 ){
                        SendInformationBack (list);
                        PrintTransaction (list,dateP2);
                        }else {System.Console.WriteLine( "Invalid transaction at that time");}  
                    }
                    break;
                case 7:
                    MainView.GenerateMenu ();
                    break;
                default:
                    System.Console.WriteLine ("Wrong choice");
                    break;
            }
            Console.WriteLine ("Press enter to continue!");
            Console.ReadLine ();
            GetTransactionByTime ();
        }
        string type;

        AccountModel model = new AccountModel ();
        public void SendInformationBack (List<Transaction> list) {

            Console.WriteLine ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}",
                "ID", "Time", "Transaction Type", "Receiver", "Sender", "Content", "Amount");
            foreach (var trans in list) {
                Account accountReceiver = model.GetAccountByAccountNumber (trans.ReceiverAccountNumber);
                Account accountSender = model.GetAccountByAccountNumber (trans.SenderAccountNumber);
                if (trans.Type.Equals (Transaction.TransactionType.DEPOSIT)) {
                    type = "DEPOSIT";
                } else if (trans.Equals (Transaction.TransactionType.WITHDRAW)) {
                    type = "WITHDRAW";
                }
                if (trans.SenderAccountNumber.Equals (trans.ReceiverAccountNumber)) {
                    System.Console.WriteLine ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}",
                        trans.Id, trans.CreatedAt, type, "", "", trans.Content, trans.Amount);
                } else if (trans.SenderAccountNumber == (Program.currentLoggedIn.AccountNumber)) {
                    System.Console.WriteLine ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}",
                        trans.Id, trans.CreatedAt, "TRANSFER", accountReceiver.FullName, "You", trans.Content, trans.Amount);

                } else if (trans.ReceiverAccountNumber == (Program.currentLoggedIn.AccountNumber)) {
                    System.Console.WriteLine ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}",
                        trans.Id, trans.CreatedAt, "TRANSFERED", "You", accountSender.FullName, trans.Content, trans.Amount);
                }
            }
        }
        public bool PrintTransaction (List<Transaction> list,string date) {

            System.Console.WriteLine ("Do you want to print the transactions? (Y|N)");
            var choice = Console.ReadLine ();
            if (choice.Equals ("Y")) {
                System.Text.StringBuilder text = new System.Text.StringBuilder ();
                text.Append (String.Format ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}{7}",
                    "ID", "Time", "Transaction Type", "Receiver", "Sender", "Content", "Amount", Environment.NewLine));
                foreach (var trans in list) {
                    Account accountReceiver = model.GetAccountByAccountNumber (trans.ReceiverAccountNumber);
                    Account accountSender = model.GetAccountByAccountNumber (trans.SenderAccountNumber);
                    if (trans.Type.Equals (Transaction.TransactionType.DEPOSIT)) {
                        type = "DEPOSIT";
                    } else if (trans.Type.Equals (Transaction.TransactionType.WITHDRAW)) {
                        type = "WITHDRAW";
                    }
                    if (trans.SenderAccountNumber.Equals (trans.ReceiverAccountNumber)) {
                        text.Append (String.Format ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}{7}",
                            trans.Id, trans.CreatedAt, type, "", "", trans.Content, trans.Amount, Environment.NewLine));

                    } else if (trans.SenderAccountNumber == (Program.currentLoggedIn.AccountNumber)) {
                        text.Append (String.Format ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}{7}",
                            trans.Id, trans.CreatedAt, "TRANSFER", accountReceiver.FullName, "You", trans.Content, trans.Amount, Environment.NewLine));

                    } else if (trans.ReceiverAccountNumber == (Program.currentLoggedIn.AccountNumber)) {
                        text.Append (String.Format ("{0,38}|{1,25}|{2,20}|{3,15}|{4,15}|{5,25}|{6,10}{7}",
                            trans.Id, trans.CreatedAt, "TRANSFERED", "You", accountSender.FullName, trans.Content, trans.Amount, Environment.NewLine));

                    }
                }
                System.IO.File.WriteAllText (@"F:\C#\spring_hero_bank\SpringHeroBank\"+date+".txt", text.ToString ());
                System.Console.WriteLine ("Print Transaction Succeed!");
                return true;
            }
            return false;
        }
        public bool ValidateDate (int day, int month, int year) {
            var currentYear = Int32.Parse (DateTime.Now.Year.ToString ());
            string currentDate = DateTime.Today.ToString("yyyy-mm-dd");
            string inputDate = year + "-" + month + "-" + day;
            var compareDate = new DateTime (year, month, day);
            if (compareDate > DateTime.Today || day <= 0 || day > 31 || month > 12 || month <= 0 || year > currentYear || year < 1000) {
                System.Console.WriteLine ("Wrong date. Please enter right date");
                return false;
            }
            return true;
        }
    }
}