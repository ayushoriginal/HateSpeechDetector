using Foundation;
using System;
using UIKit;
using NaturalLanguage;
using System;
using System.Runtime.InteropServices;
using Foundation;



using Foundation;
using System;
using System.Linq;
using UIKit;


using System.IO;
using System.Collections;
using System.Collections.Generic;

using CoreML;
using CoreVideo;

using System.Diagnostics;
using NaturalLanguage;
using System.Text;

using LemmaSharp;


namespace XamarinNL
{

    public partial class LanguageTokenizerViewController : UIViewController, IUITextFieldDelegate
    {
        private readonly hate_coreml hate_model = new hate_coreml();
        const string ShowTokensSegue = "ShowTokensSegue";

        NSValue[] tokens;

        public LanguageTokenizerViewController(IntPtr handle) : base(handle) {

         }





        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            var destination = segue.DestinationViewController as LanguageTokenizerTableViewController;
            if (destination != null)
            {
                destination.Tokens = tokens;
                destination.Text = UserInput.Text;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UserInput.Delegate = this;
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            UserInput.ResignFirstResponder();
            return true;
        }

        void ShowTokens(NLTokenUnit unit)
        {
            if (!String.IsNullOrWhiteSpace(UserInput.Text))
            {

                var tokenizer = new NLTokenizer(unit);
                tokenizer.String = UserInput.Text;
                var range = new NSRange(0, UserInput.Text.Length);
                var k = tokenizer.GetTokens(range); // returns Foundation.NSValue[]
                tokens = k;
                string ss = "hello";


                //tokens[0] = NSValueMaker.GetValue(ss);
                Console.WriteLine(tokens[0]);

                Console.WriteLine("Sample Sample!");




                //await DisplayAlert("Greeting");

                //PerformSegue(ShowTokensSegue, this);
            }
        }

        partial void FindWordsButton_TouchUpInside(UIButton sender)
        {


            string mytext = UserInput.Text;

            List<string> stopword = new List<string>();

            foreach (var line in File.ReadLines("stopword.txt"))
            {
                stopword.Add(line);


            }




            //string[] stopword = new string[] { "user", "a", "she", "should", "few", "what", "their", "on", "this", "is" };
            int count_it = 0;
            //foreach (string r_t in raw_text_string)
            //string rt = mytext;
            List<double> predicted_labels = new List<double>();
            string hate_speech_var = "NOT OFFENSIVE";
            double prob = 0.0;

            {
                count_it += 1;
                Console.WriteLine(count_it.ToString());
                string raw_text = mytext;
                string input_text = "a " + raw_text;

                //NOTE:
                // 1. If the input_text starts with @ as its first character, the Xamarin Natual Language namespace based Tokenizer breaks
                // ERROR: SecTaskLoadEntitlements failed error=22 cs_flags=200, pid=30250
                // ERROR: SecTaskCopyDebugDescription: MarsHabitatPrice[30250]/0#-1 LF=0
                // To tackle this, add a character 'a' in front of the string which will become a separate token and will be removed in stopwords (or remove explicitly)

                //causes the escape sequence to be ignore


                //STAGE 1: Tokenization + Lowercase + Remove Punctionation
                // TODO: Make sure that the apostrophe's remaining inside the word due to wierd tokenization is removed in stopword stage
                // TODO: Check Apple's documentation/ github repo on this issue
                //    def tokenize(self):
                //      from nltk import word_tokenize
                //      for i, tweet in tqdm(enumerate(self.data), 'Tokenization'):
                //      self.data[i] = word_tokenize(tweet.lower())
                //      return self.data
                //NSValue[] tokens;
                Console.WriteLine("**START**");

                string[] tokens_str = null;

                if (!String.IsNullOrWhiteSpace(input_text))
                {
                    var tokenizer = new NLTokenizer(NLTokenUnit.Word); // or could be NLTokenUnit.Sentence for sentence tokenization

                    tokenizer.String = input_text;
                    var range = new NSRange(0, input_text.Length);
                    //[P]Console.WriteLine("RANGE:");
                    //[P]Console.WriteLine(range);



                    NSValue[] tokens = tokenizer.GetTokens(range); // Returns Array of NSValue Objects, each wrapping an NSRange value
                                                                   //Attempting to print the string from an individual NSValue Object

                    //Attempt 1: Try to Directly convert using ToString method- FAILED
                    //Console.WriteLine("1) Tokens: ");
                    //Console.WriteLine(tokens[0].ToString()); // Output: NSRange: {0, 3}


                    //Attempt 2: Since Previous attempt gives an NSRange value, try to convert that again using ToString()- FAILED
                    //Console.WriteLine("2) Tokens: ");
                    //Console.WriteLine(tokens[0].ToString().ToString()); // Output: NSRange: {0, 3}


                    //Attempt 3: Since, NSRange = structure used to describe a portion of a series, such as characters in a string, we
                    // try to extract the RangeValue from NSValue Object and use its location and length to get a substring out of joined array of tokens - PASSED
                    //Console.WriteLine("3) Tokens: ");

                    //NSRange rr = tokens[0].RangeValue; // Extract Range Value
                    //string s = input_text.Substring((int)rr.Location, (int)rr.Length);  // Error: Object reference not set to an instance of an object
                    //Console.WriteLine(s); 

                    tokens_str = new string[tokens.Length];

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        NSRange rr = tokens[i].RangeValue; // Extract Range Value
                        string s = input_text.Substring((int)rr.Location, (int)rr.Length);
                        tokens_str[i] = s.ToLower();
                        //[P]Console.WriteLine(s);

                    }


                    //NSRange rr = tokens[0].RangeValue; // Extract Range Value
                    //string s = input_text.Substring((int)rr.Location, (int)rr.Length);  // Error: Object reference not set to an instance of an object
                    //Console.WriteLine(s); 




                    //NSRange rr = tokens[0].RangeValue;
                    //string s = Text.Substring((int)rr.Location, (int)rr.Length);

                    //string temp11;

                    //[P]for (int i = 0; i < tokens_str.Length; i++)
                    //[P]{
                    //[P]    temp11 = tokens_str[i] + " ";
                    //[P]Console.Write(temp11);
                    //[P]}

                }
                Console.WriteLine("**END**");






                //EXPECTED OUTPUT:
                // ['@', 'user', 'she', 'should', 'ask', 'a', 'few', 'native', 'americans', 'what', 'their', 'take', 'on', 'this', 'is', '.']
                // ['@', 'user', '@', 'user', 'go', 'home', 'you', '’', 're', 'drunk', '!', '!', '!', '@', 'user', '#', 'maga', '#', 'trump2020', '👊🇺🇸👊', 'url']
                // ['amazon', 'is', 'investigating', 'chinese', 'employees', 'who', 'are', 'selling', 'internal', 'data', 'to', 'third-party', 'sellers', 'looking', 'for', 'an', 'edge', 'in', 'the', 'competitive', 'marketplace', '.', 'url', '#', 'amazon', '#', 'maga', '#', 'kag', '#', 'china', '#', 'tcot']


                //ACTUAL OUPUT:
                // a user she should ask a few native americans what their take on this is
                // a user user go home you’re drunk user maga trump2020 👊 🇺🇸 👊 url
                // a amazon is investigating chinese employees who are selling internal data to third party sellers looking for an edge in the competitive marketplace url amazon maga kag china tcot


                //TODO: TEST THE TOKENIZER EXTENSIVELY AND GET BUGS


                //STAGE 2: Stop-Word Removal

                //def remove_stopwords(self):
                //  from nltk.corpus import stopwords
                //  import re
                //  stop = set(stopwords.words("english"))
                //  noise = ['user']
                //  for i, tweet in tqdm(enumerate(self.data), 'Stopwords Removal'):
                //      self.data[i] = [w for w in tweet if w not in stop and not re.match(r"[^a-zA-Z\d\s]+", w) and w not in noise]
                //  return self.data




                var stopword_set = new HashSet<string>(stopword);  //Hashset of Stopwords

                // Let t_tokens_str be the tokenized version of the string

                //string[] t_tokens_str = new string[] { "amazon","are","investigating","who","chinese","is",".","who"};

                List<string> stop_tokens_str = new List<string>();

                foreach (string word in tokens_str)
                {
                    if (stopword_set.Contains(word))
                        continue;
                    else
                        stop_tokens_str.Add(word);

                }
                Console.WriteLine("AFTER STOPWORD REMOVAL");

                //[P]foreach (string word in stop_tokens_str)
                //[P]Console.WriteLine(word);


                //StringBuilder input = new StringBuilder("Did you try this yourself before asking");
                //foreach (string word in tokens_str)
                //{
                //    if word in stopword
                //}
                //Console.WriteLine(input);

                //EXPECTED OUTPUT:
                // ['ask', 'native', 'americans', 'take'], 
                // ['go', 'home', 'drunk', 'maga', 'trump2020', 'url'], 
                // ['amazon', 'investigating', 'chinese', 'employees', 'selling', 'internal', 'data', 'third-party', 'sellers', 'looking', 'edge', 'competitive', 'marketplace', 'url', 'amazon', 'maga', 'kag', 'china', 'tcot']

                //ACTUAL OUTPUT:
                // ['ask', 'native', 'americans', 'take'] 

                //STAGE 3: Lemmatization

                //def lemmatize(self):
                //    from nltk.stem import WordNetLemmatizer
                //    wnl = WordNetLemmatizer()
                //    for i, tweet in tqdm(enumerate(self.data), 'Lemmatization'):
                //        for j, word in enumerate(tweet):
                //            self.data[i][j] = wnl.lemmatize(word, pos = self.get_pos(word))
                //    return self.data

                var currentDirectory = Directory.GetCurrentDirectory();


                //string[] fileArray = Directory.GetFiles(currentDirectory);


                //foreach (string f in fileArray)
                //{ 
                //    Console.WriteLine();
                //}

                var dataFilePath = string.Format("{0}/{1}", currentDirectory, "full7z-mlteast-en.lem"); // maybe add @




                //Console.WriteLine(File.Exists(dataFilePath) ? "File exists." : "File does not exist.");

                //var path = "Resources/full7z-mlteast-en.lem";
                var stream = File.OpenRead(dataFilePath);
                var lemmatizer = new Lemmatizer(stream); //Load Lemmatizer with the given dataFilePath



                List<string> lemma_tokens_str = new List<string>();

                foreach (string word in stop_tokens_str)
                {
                    var result2 = lemmatizer.Lemmatize(word);
                    lemma_tokens_str.Add(result2);

                }
                Console.WriteLine("AFTER LEMMATIZATION");

                //[P]foreach (string word in lemma_tokens_str)
                //[P]Console.WriteLine(word);





                //EXPECTED OUTPUT:
                // ['ask', 'native', 'american', 'take']
                // ['go', 'home', 'drunk', 'maga', 'trump2020', 'url']
                // ['amazon', 'investigate', 'chinese', 'employee', 'sell', 'internal', 'data', 'third-party', 'seller', 'look', 'edge', 'competitive', 'marketplace', 'url', 'amazon', 'maga', 'kag', 'china', 'tcot']


                //ACTUAL OUTPUT:
                // ['ask','native','american','take']


                // TODO: EDGE CASES- OOV WORDS
                // STAGE 4: Count-Vectorization
                //FileStream meta_stream = File.Open("metadata_length_tweet_size_vocab.txt", FileMode.Open);


                //INPUT: lemma_tokens_str which is List<string>

                int len_tweet = 0;
                int size_vocab = 0;


                // Tweet Length and Size of Vocab
                foreach (var line in File.ReadLines("metadata_length_tweet_size_vocab.txt"))
                {
                    string[] temp = line.Split(" ");
                    len_tweet = Convert.ToInt32(temp[0]);
                    size_vocab = Convert.ToInt32(temp[1]);

                }

                // Vocab Mapping

                var word_code = new Dictionary<string, int>();

                string temp_word = null;
                int temp_code = 0;

                foreach (var line in File.ReadLines("vocab_mapping.txt"))
                {
                    string[] temp2 = line.Split(" ");
                    temp_word = temp2[0];
                    temp_code = Convert.ToInt32(temp2[1]);
                    if (word_code.ContainsKey(temp_word))
                    {
                        continue;
                    }
                    else
                    {
                        word_code.Add(temp_word, temp_code);
                    }


                }

                //[P]foreach (string key in word_code.Keys)
                //[P]{
                //[P]Console.WriteLine(String.Format("{0}: {1}", key, word_code[key]));
                //[P]}

                // Vocab Mapping has been loaded to a dictionary


                // Now create vectors corresponding to each tweet


                //Convert list of string to mapped double array

                double[] example = new double[len_tweet];//Final Array automatically assigned to 0
                int k = 0;
                int m = 0;
                string possible_key = null;
                while (k < lemma_tokens_str.Count)
                {
                    //[P]Console.Write("lemma_tokens_str.Count: ");
                    //[P]Console.WriteLine(lemma_tokens_str.Count); //21

                    //[P]Console.Write("k: ");
                    //[P]Console.WriteLine(k); //3

                    //[P]foreach (var ss2 in lemma_tokens_str)
                    //[P]{
                    //[P]Console.Write(ss2);
                    //[P]}

                    //Console.WriteLine("\n Posiala a");
                    possible_key = lemma_tokens_str[k];
                    //[P]Console.WriteLine("\n Possible Key:");
                    //[P]Console.WriteLine(possible_key);
                    if (word_code.ContainsKey(possible_key))
                    {

                        example[m] = word_code[lemma_tokens_str[k]];
                        m += 1;
                        k += 1;
                    }
                    else
                    {
                        k += 1;
                        continue;
                    }

                }

                //[P]foreach(var vv in example)
                //[P]{
                //[P]Console.WriteLine(vv);
                //[P]}







                //*****PREPROCESSING ENDS HERE*****//


                // Initialize MLMultiArray
                //Swift Sytax doesn't work
                //MLMultiArray temp1 = MLMultiArray(shape:[1, 44], MLMultiArrayDataType: MLMultiArrayDataType.double);

                //double[] example = {6620,  1912,  9987,  4577, 10130, 13048,  5191, 10897,   208, 13091,  9104,    0,
                //     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
                //     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
                //     0,     0,     0,     0,     0,     0,     0,     0 };

                //TODO: Figure out how to convert 'example' into a MLMultiArray called 'temp1'


                //******START TODO*******//

                nint[] ns_sum = new nint[] { 44, 1, 1 }; // Try to exchange dimensions

                //MLMultiArray temp1 = new MLMultiArray(ns_sum, MLMultiArrayDataType.Double, out NSError error3);
                //for (int i = 0; i < 44; i++)
                //{
                // Convert each example[i] to NSNumber
                //temp1.SetObject(new NSNumber(example[i]),i);

                //Console.WriteLine(i);
                //}
                //var example = new double[] {6620,  1912,  9987,  4577, 10130, 13048,  5191, 10897,   208, 13091,  9104,    0,
                //     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
                //     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
                //     0,     0,     0,     0,     0,     0,     0,     0 };

                MLMultiArray temp1 = new MLMultiArray(ns_sum, MLMultiArrayDataType.Double, out NSError error3);

                var narray = new NSNumber[example.Length];
                for (int i = 0; i < narray.Length; i++)
                    narray[i] = NSNumber.FromDouble(example[i]);

                for (int i = 0; i < narray.Length; i++)
                    temp1.SetObject(narray[i], i);


                //MLMultiArray temp1 = new MLMultiArray(narray, MLMultiArrayDataType.Double, out NSError error3);


                //******END TODO*******//
                Console.WriteLine("EXAMPLE-OUTPUT");

               

                var hate_coremlOutput = this.hate_model.GetPrediction(temp1, out NSError error2);


                if (error3 != null)
                {
                    throw new Exception("Unexpected runtime error.");
                }

                if (error2 != null)
                {

                    throw new Exception("Error with Hate Model during Runtime.\n");
                }

                var hate_prob = hate_coremlOutput.Output1;


                Console.WriteLine("Hate Probability Score: ");


                Console.WriteLine(hate_prob);

                //            List<double> predicted_labels = new List<double>();
                //              string hate_speech_var = "NOT HATE SPEECH";

                predicted_labels.Add(hate_prob[0].DoubleValue);
                predicted_labels.Add(hate_prob[1].DoubleValue);

                if (hate_prob[0].DoubleValue > hate_prob[1].DoubleValue)
                {
                    hate_speech_var = "OFFENSIVE";
                    prob = hate_prob[0].DoubleValue;

                }
                else
                {
                    prob = hate_prob[1].DoubleValue;
                }
            }





            string title_text = "TEXT IS " + hate_speech_var;
            string body_text = "Probability of being " + hate_speech_var + " is " + prob.ToString();
            var okAlertController = UIAlertController.Create(title_text, body_text, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            PresentViewController(okAlertController, true, null);
        }
    }
}