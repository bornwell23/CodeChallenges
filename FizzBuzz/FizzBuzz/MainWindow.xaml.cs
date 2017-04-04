using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace FizzBuzz
{
    public partial class MainWindow : Window
    {
        public class FizzObj
        {
            public int fizzNum;
            public string fizzWord;
            public int FizzNum { get { return fizzNum; } set { fizzNum = value; } }
            public string FizzWord { get { return fizzWord; } set { fizzWord = value; } }

            public FizzObj(int num, string word)
            {
                fizzNum = num;
                fizzWord = word;
            }
        }

        List<FizzObj> FizzListItemSource = new List<FizzObj>();
        Thread FizzThread;

        public MainWindow()
        {
            InitializeComponent();
            FizzList.ItemsSource = FizzListItemSource;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int num = 0;
            try
            {
                num = Int32.Parse(EnterNumber.Text);
                if (FizzListItemSource.FirstOrDefault(fizzObj => fizzObj.fizzNum.Equals(num)) != null) //not the best practice to do this in the UI thread, but this data should be minimal, so this should not be a problem. However, for a real application, this would run in a background thread instead of the UI.
                {
                    throw new Exception("Repeat data");
                }
            }
            catch (Exception)
            {
                //no need to do anything with the exception in a program of this simplicity.
                OutputPane.Text = "Failed to get a proper number from the box. Please enter a valid unique integer";
                return;
            }
            string word = "";
            try
            {
                word = EnterWord.Text;
                if (word.Equals(string.Empty))
                {
                    throw new Exception("Bad data");
                }
                else if (FizzListItemSource.FirstOrDefault(fizzObj => fizzObj.fizzWord.Equals(word)) != null) //not the best practice to do this in the UI thread, but this data should be minimal, so this should not be a problem. However, for a real application, this would run in a background thread instead of the UI.
                {
                    throw new Exception("Repeat data");
                }
            }
            catch (Exception)
            {
                //no need to do anything with the exception in a program of this simplicity.
                OutputPane.Text = "Failed to get a proper word from the box. Please enter a valid unique string";
                return;
            }
            FizzListItemSource.Add(new FizzObj(num, word));
            FizzList.Items.Refresh();
        }

        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            FizzListItemSource.Remove((FizzObj)FizzList.SelectedItem);
            FizzList.Items.Refresh();
        }

        private void FizzBuzz_Click(object sender, RoutedEventArgs e)
        {
            if (FizzThread != null)
            {
                try
                {
                    FizzThread.Abort();
                }
                catch(Exception)
                {
                    //I know this will be called, but I don't care because it's entirely expected
                }
            }
            FizzThread = new Thread(() =>
            {
                RunFizzBuzz();
            });
            FizzThread.Start();
        }

        private void RunFizzBuzz()
        {
            try
            {
                int max = 0;
                FizzWindow.Dispatcher.Invoke(new Action(() =>
                {
                    max = Int32.Parse(EnterMax.Text);
                    OutputPane.Text = "";
                }));
                if (max == 0)
                {
                    FizzWindow.Dispatcher.Invoke(new Action(() =>
                    {
                        OutputPane.Text = "Please enter a non-zero maximum";
                    }));
                    return;
                }
                else if (max < 0)
                {
                    for (int i = -1; i >= max; --i)
                    {
                        try
                        {
                            string tempOut = FizzListItemSource.Where(fizz => (i % fizz.fizzNum == 0)).Select(obj => obj.fizzWord).Aggregate((current, next) => current + "" + next); //There is likely a more optimized way, but this looked cooler. This line adds up all the strings that correspond to numbers that the current iteration is divisible by
                            if (!tempOut.Equals(""))
                            {
                                FizzWindow.Dispatcher.Invoke(new Action(() =>
                                {
                                    OutputPane.Text = tempOut += "\n";
                                }));
                            }
                            else
                            {
                                throw new Exception("output is empty");
                            }
                        }
                        catch (Exception)
                        {
                            FizzWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                OutputPane.Text += i + "\n";
                            }));
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= max; ++i)
                    {
                        try
                        {
                            string tempOut = FizzListItemSource.Where(fizz => (i % fizz.fizzNum == 0)).Select(obj => obj.fizzWord).Aggregate((current, next) => current + "" + next); //There is likely a more optimized way, but this looked cooler. This line adds up all the strings that correspond to numbers that the current iteration is divisible by
                            if (!tempOut.Equals(""))
                            {
                                FizzWindow.Dispatcher.Invoke(new Action(() =>
                                {
                                    OutputPane.Text += tempOut + "\n";
                                }));
                            }
                            else
                            {
                                throw new Exception("output is empty");
                            }
                        }
                        catch (Exception)
                        {
                            FizzWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                OutputPane.Text += i + "\n";
                            }));
                        }
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    FizzWindow.Dispatcher.Invoke(new Action(() =>
                    {
                        OutputPane.Text = "There was an error when attempting to write the output of the fizzbuzz program";
                    }));
                }
                catch(Exception)
                {
                    //it's possible that this breaks when closing the program mid call, so this is to ensure that the user never sees that error
                }
            }
        }

        private void FizzWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (FizzThread != null)
            {
                try
                {
                    FizzThread.Abort(); //don't want to leave an extra process lying around...
                }
                catch (Exception)
                {
                    //I know this will be called, but I don't care because it's entirely expected
                }
            }
        }
    }
}
