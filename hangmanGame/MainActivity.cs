
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace hangmanGame
{
    [Activity(Label = "HANGMAN GAME",  Icon = "@drawable/icon", ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]
    public class MyActivity : Activity
    {
        Button btna, btnb, btnc, btnd, btne, btnf, btng, btnh, btni, btnj, btnk, btnl, btnm, btnn, btno, btnp, btnq, btnr, btns, btnt, btnu, btnv, btnw, btnx, btny, btnz;
        string letterGuessed, newString, selectedWord, stringCheck, stringCurrent, stringtemp, clearAllTxt;
        int i, hangCount, lettersFound = 0, userScore;
        ListView lstToDoList;
        List<ToDo> myList;
        static string dbName = "hangmanwords.sqlite";
        string dbPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);
        DatabaseManager objDb;
        char j;
        Boolean clickable, won;
        TextView GuessWordTextView, chancesLeftCountView, wonORlostTextView, restartgame, textTapLetter, score, scoretext, scoretextLevel;
        ImageView hangmanImage, blockoutImg;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //SET TEXTVIEW
            GuessWordTextView =    (TextView)FindViewById(Resource.Id.GUESSWORD);
            textTapLetter =        (TextView)FindViewById(Resource.Id.WORD);
            chancesLeftCountView = (TextView)FindViewById(Resource.Id.chances);
            wonORlostTextView =    (TextView)FindViewById(Resource.Id.chancesText);
            restartgame =          (TextView)FindViewById(Resource.Id.restartgame);
            hangmanImage =         (ImageView)FindViewById(Resource.Id.hangmanimage);
            blockoutImg =          (ImageView)FindViewById(Resource.Id.blockoutImg);
            score =                (TextView)FindViewById(Resource.Id.score);
            scoretext =            (TextView)FindViewById(Resource.Id.scoretext);
            scoretextLevel =      (TextView)FindViewById(Resource.Id.scoretextLevel);
            clearAllTxt = null;
            score.Text = (clearAllTxt);
            scoretext.Text = (clearAllTxt);
            scoretextLevel.Text = (clearAllTxt);
            chancesLeftCountView.Text = (clearAllTxt);
            hangmanImage.SetImageResource(Resource.Drawable.img0);
            lstToDoList = FindViewById<ListView>(Resource.Id.listView1);
            lstToDoList.ItemClick += lstToDoList_ItemClick;
            CopyDatabase();
            objDb = new DatabaseManager();
            myList = objDb.ViewAll();
            selectedWord = myList[0].WORD; // 1:: EXTRACT RANDOM WORD - from the list of words CALLED from dbmanager : db command line
            Console.WriteLine("I'm working  WORD...." + selectedWord);
            wordFillStars();
            hangCount = 0;
            lstToDoList.Adapter = new DataAdapter(this, myList);
            InitializeControls();
        }

        // ****************************************************************************************************
        //  2:: DONE - FILL blankWord array with **** same length as random WORD & POPULATE TextView:GUESSWORD     
        public void wordFillStars()  //RANDOMIZE WORD
        {
            char[] wordArrayNew = selectedWord.ToCharArray();
            newString = new string('*', wordArrayNew.Length);
            stringCurrent = newString;  // sets up stringCurrent in the first instance
            GuessWordTextView.Text = (newString);        
        }
        // ****************************************************************************************************

        public void checkLetterWord()
        {
            stringCheck = stringCurrent; // compare existing string in loop below for checkGuessedLetter
            checkGuessedLetter();
            i = 0;
            GuessWordTextView.Text = (stringCurrent);								           
        }

        public void checkGuessedLetter()
        {
            stringtemp = new string('*', selectedWord.Length);  // ***************  
            char[] result = stringtemp.ToCharArray();  
            // Tell the user the number of letters through the dashes                     
            for (i = 0; i < selectedWord.Length; i++)
            {  
                char letterGuess = letterGuessed[0];
                char letterInString = selectedWord[i];

                if (letterGuess == letterInString)
                {
                    result[i] = selectedWord[i];  //CORRECT LETTER                    
                }
                else if ((letterGuess == stringCheck[i]) || (stringtemp[i] == stringCheck[i]))
                {
                    result[i] = stringtemp[i];   // * WRONG LETTER
                }
                else 
                {
                    result[i] = selectedWord[i]; //CORRECT LETTER
                }
                stringCurrent = new string(result);
                GuessWordTextView.Text = (stringCurrent);
            }
            return;
        }
        // ****************************************************************************************************
        // 3:: SELECT LETTER FROM ALPHA compare with all letters in selectWord Array
        public void letterSelect()
        {   string strcharacter = null; 
            string restartgameTxt = "PRESS BACKBUTTON TO RESTART GAME";
            char[] checkword = selectedWord.ToCharArray();
            strcharacter = checkword.ToString();
            if (selectedWord.Contains(letterGuessed))
            {
                checkLetterWord();
                lettersFound++;
                if (lettersFound == selectedWord.Length)
                {

                    string winningWord = "IS THE WORD... WELL DONE!!";
                    string youWon = "YOU HAVE WON !!!! ... HANGMAN";
                    won = true;
                    chancesLeftCountView.Text = (clearAllTxt);
                    textTapLetter.Text = (youWon);
                    chancesLeftCountView.Text = (clearAllTxt); 
                    restartgame.Text = (restartgameTxt);
                    wonORlostTextView.Text = (winningWord);
                    getScore();
                    blockoutImg.SetImageResource(Resource.Drawable.img000); 
                    disableAlphabetBtns();   //disable alphabet buttons
                    hangmanImage.SetImageResource(Resource.Drawable.img0a);
                }
            }
            else
            {
                hangCount = hangCount + 1;
                if (hangCount >= 12)
                {
                    string youLost = "YOU HAVE LOST !!!! ... HANGMAN";
                    string revealWord = "IS THE WORD... UNLUCKY!!"; 
                    textTapLetter.Text = (youLost);
                    chancesLeftCountView.Text = (clearAllTxt); 
                    wonORlostTextView.Text = (revealWord);
                    GuessWordTextView.Text = (selectedWord);
                    restartgame.Text = (restartgameTxt);
                    getScore();
                    disableAlphabetBtns();
                    blockoutImg.SetImageResource(Resource.Drawable.img00);  //so buttons are not clickable
                    hangmanImage.SetImageResource(Resource.Drawable.img12);                 
                }
                else
                {
                    string chancesHangCount = Convert.ToString(hangCount);
                    chancesLeftCountView.Text = (chancesHangCount);
                    if     (hangCount == 1)  {hangmanImage.SetImageResource(Resource.Drawable.img1);}
                    else if (hangCount == 2) {hangmanImage.SetImageResource(Resource.Drawable.img2);}
                    else if (hangCount == 3)  {hangmanImage.SetImageResource(Resource.Drawable.img3);}
                    else if (hangCount == 4)  {hangmanImage.SetImageResource(Resource.Drawable.img4);}
                    else if (hangCount == 5)  {hangmanImage.SetImageResource(Resource.Drawable.img5);}
                    else if (hangCount == 6) {hangmanImage.SetImageResource(Resource.Drawable.img6);}
                    else if (hangCount == 7) {hangmanImage.SetImageResource(Resource.Drawable.img7);}
                    else if (hangCount == 8) {hangmanImage.SetImageResource(Resource.Drawable.img8);}
                    else if (hangCount == 9) {hangmanImage.SetImageResource(Resource.Drawable.img9);}
                    else if (hangCount == 10) {hangmanImage.SetImageResource(Resource.Drawable.img10);}
                    else if (hangCount == 11) {hangmanImage.SetImageResource(Resource.Drawable.img11a);}
                }
            }
            return;
        }
        public void getScore() //GIVES HIGH SCORE ON WHAT IS REMANING FROM hangCount - less guesses more points
        {
            if (hangCount >= 6)
            {
                userScore = userScore + 10; 
            }
            else if (hangCount == 5)
            {
                userScore = userScore + 20;
            }
            else if (hangCount == 4)
            {
                userScore = userScore + 30;
            }
            else if (hangCount == 3)
            {
                userScore = userScore + 40;
            }
            else if (hangCount == 2)
            {
                userScore = userScore + 50;
            }
            else if (hangCount == 1)
            {
                userScore = userScore + 60;
            }
            Console.WriteLine("HANGCOUNT =" + hangCount + " USER SCORE  =  " + userScore);
            string scoreCount = Convert.ToString(userScore); 
            string scoreViewText = "Your score is : ";
            string scoretextLevelText = "Out of 60";
            score.Text = (scoreCount);
            scoretextLevel.Text = (scoretextLevelText);
            scoretext.Text = (scoreViewText);
            //blockoutImg.SetImageResource(Resource.Drawable.img1); 
            return;
        }

        // ****************************************************************************************************
        public void InitializeControls()
        {   btna = FindViewById<Button>(Resource.Id.btna); btnb = FindViewById<Button>(Resource.Id.btnb); btnc = FindViewById<Button>(Resource.Id.btnc);
            btnd = FindViewById<Button>(Resource.Id.btnd);
            btne = FindViewById<Button>(Resource.Id.btne); btnf = FindViewById<Button>(Resource.Id.btnf);
            btng = FindViewById<Button>(Resource.Id.btng); btnh = FindViewById<Button>(Resource.Id.btnh); btni = FindViewById<Button>(Resource.Id.btni);
            btnj = FindViewById<Button>(Resource.Id.btnj); btnk = FindViewById<Button>(Resource.Id.btnk); btnl = FindViewById<Button>(Resource.Id.btnl);
            btnm = FindViewById<Button>(Resource.Id.btnm); btnn = FindViewById<Button>(Resource.Id.btnn); btno = FindViewById<Button>(Resource.Id.btno);
            btnp = FindViewById<Button>(Resource.Id.btnp); btnq = FindViewById<Button>(Resource.Id.btnq); btnr = FindViewById<Button>(Resource.Id.btnr);
            btns = FindViewById<Button>(Resource.Id.btns); btnt = FindViewById<Button>(Resource.Id.btnt); btnu = FindViewById<Button>(Resource.Id.btnu);
            btnv = FindViewById<Button>(Resource.Id.btnv); btnw = FindViewById<Button>(Resource.Id.btnw); btnx = FindViewById<Button>(Resource.Id.btnx);
            btny = FindViewById<Button>(Resource.Id.btny); btnz = FindViewById<Button>(Resource.Id.btnz);
            btna.Click += onBtnaClick; btnb.Click += onBtnbClick; btnc.Click += onBtncClick; btnd.Click += onBtndClick;
            btne.Click += onBtneClick;
            btnf.Click += onBtnfClick; btng.Click += onBtngClick; btnh.Click += onBtnhClick; btni.Click += onBtniClick; btnj.Click += onBtnjClick;
            btnk.Click += onBtnkClick; btnl.Click += onBtnlClick; btnm.Click += onBtnmClick; btnn.Click += onBtnnClick; btno.Click += onBtnoClick;
            btnp.Click += onBtnpClick; btnq.Click += onBtnqClick; btnr.Click += onBtnrClick; btns.Click += onBtnsClick; btnt.Click += onBtntClick;
            btnu.Click += onBtnuClick; btnv.Click += onBtnvClick; btnw.Click += onBtnwClick; btnx.Click += onBtnxClick; btny.Click += onBtnyClick;
            btnz.Click += onBtnzClick;
        }
        //game won or lost must disable all buttons left but what are they?... disable all
        public void disableAlphabetBtns()
        {
            btna.Visibility = ViewStates.Invisible; btnb.Visibility = ViewStates.Invisible; btnc.Visibility = ViewStates.Invisible; btnd.Visibility = ViewStates.Invisible;
            btne.Visibility = ViewStates.Invisible; btnf.Visibility = ViewStates.Invisible; btng.Visibility = ViewStates.Invisible; btnh.Visibility = ViewStates.Invisible;
            btni.Visibility = ViewStates.Invisible; btnj.Visibility = ViewStates.Invisible; btnk.Visibility = ViewStates.Invisible; btnl.Visibility = ViewStates.Invisible;
            btnm.Visibility = ViewStates.Invisible; btnn.Visibility = ViewStates.Invisible; btno.Visibility = ViewStates.Invisible; btnp.Visibility = ViewStates.Invisible;
            btnq.Visibility = ViewStates.Invisible; btnr.Visibility = ViewStates.Invisible; btns.Visibility = ViewStates.Invisible; btnt.Visibility = ViewStates.Invisible;
            btnu.Visibility = ViewStates.Invisible; btnv.Visibility = ViewStates.Invisible; btnw.Visibility = ViewStates.Invisible; btnx.Visibility = ViewStates.Invisible; 
            btny.Visibility = ViewStates.Invisible; btnz.Visibility = ViewStates.Invisible;
        }

        private void onBtnzClick(object sender, EventArgs e)
        {
            clickable = false;
          //  if (clickable == false) { } 
            letterGuessed = "Z";
            letterSelect();
            btnz.Enabled = false;
            btnz.Visibility = ViewStates.Invisible;
        }
        private void onBtnyClick(object sender, EventArgs e)
        {
            clickable = false;
           // if (clickable == false) { } 
            letterGuessed = "Y";
            letterSelect();
            btny.Enabled = false;
            btny.Visibility = ViewStates.Invisible;
        }
        private void onBtnxClick(object sender, EventArgs e)
        {
            clickable = false;
            // if (clickable == false) { } 
            letterGuessed = "X";
            letterSelect();
            btnx.Enabled = false;
            btnx.Visibility = ViewStates.Invisible;
        }
        private void onBtnwClick(object sender, EventArgs e)
        {
            letterGuessed = "W";
            letterSelect();
            btnw.Enabled = false;
            btnw.Visibility = ViewStates.Invisible;
        }
        private void onBtnvClick(object sender, EventArgs e)
        {
            letterGuessed = "V";
            letterSelect();
            btnv.Enabled = false;
            btnv.Visibility = ViewStates.Invisible;
        }
        private void onBtnuClick(object sender, EventArgs e)
        {
            letterGuessed = "U";
            letterSelect();
            btnu.Enabled = false;
            btnu.Visibility = ViewStates.Invisible;
        }

        private void onBtntClick(object sender, EventArgs e)
        {
            letterGuessed = "T";
            letterSelect();
            btnt.Enabled = false;
            btnt.Visibility = ViewStates.Invisible;
        }
        private void onBtnsClick(object sender, EventArgs e)
        {
            letterGuessed = "S";
            letterSelect();
            btns.Enabled = false;
            btns.Visibility = ViewStates.Invisible;
        }
        private void onBtnrClick(object sender, EventArgs e)
        {
            letterGuessed = "R";
            letterSelect();
            btnr.Enabled = false;
            btnr.Visibility = ViewStates.Invisible;
        }
        private void onBtnqClick(object sender, EventArgs e)
        {
            letterGuessed = "Q";
            letterSelect();
            btnq.Enabled = false;
            btnq.Visibility = ViewStates.Invisible;
        }
        private void onBtnpClick(object sender, EventArgs e)
        {
            letterGuessed = "P";
            letterSelect();
            btnp.Enabled = false;
            btnp.Visibility = ViewStates.Invisible;
        }
        private void onBtnoClick(object sender, EventArgs e)
        {
            letterGuessed = "O";
            letterSelect();
            btno.Enabled = false;
            btno.Visibility = ViewStates.Invisible;
        }
        private void onBtnnClick(object sender, EventArgs e)
        {
            letterGuessed = "N";
            letterSelect();
            btnn.Enabled = false;
            btnn.Visibility = ViewStates.Invisible;
        }
        private void onBtnmClick(object sender, EventArgs e)
        {
            letterGuessed = "M";
            letterSelect();
            btnm.Enabled = false;
            btnm.Visibility = ViewStates.Invisible;
        }
        private void onBtnlClick(object sender, EventArgs e)
        {
            letterGuessed = "L";
            letterSelect();
            btnl.Enabled = false;
            btnl.Visibility = ViewStates.Invisible;
        }
        private void onBtnkClick(object sender, EventArgs e)
        {
            letterGuessed = "K";
            letterSelect();
            btnk.Enabled = false;
            btnk.Visibility = ViewStates.Invisible;
        }

        private void onBtnjClick(object sender, EventArgs e)
        {
            letterGuessed = "J";
            letterSelect();
            btnj.Enabled = false;
            btnj.Visibility = ViewStates.Invisible;
        }

        private void onBtniClick(object sender, EventArgs e)
        {
            letterGuessed = "I";
            letterSelect();
            btni.Enabled = false;
            btni.Visibility = ViewStates.Invisible;
        }
        private void onBtnhClick(object sender, EventArgs e)
        {
            letterGuessed = "H";
            letterSelect();
            btnh.Enabled = false;
            btnh.Visibility = ViewStates.Invisible;
        }
        private void onBtngClick(object sender, EventArgs e)
        {
            letterGuessed = "G";
            letterSelect();
            btng.Enabled = false;
            btng.Visibility = ViewStates.Invisible;
        }
        private void onBtnfClick(object sender, EventArgs e)
        {
            letterGuessed = "F";
            letterSelect();
            btnf.Enabled = false;
            btnf.Visibility = ViewStates.Invisible;
        }

        private void onBtneClick(object sender, EventArgs e)
        {
            letterGuessed = "E";
            letterSelect();
            btne.Enabled = false;
            btne.Visibility = ViewStates.Invisible;
        }
        private void onBtndClick(object sender, EventArgs e)
        {
            letterGuessed = "D";
            letterSelect();
            btnd.Enabled = false;
            btnd.Visibility = ViewStates.Invisible;
        }
        private void onBtncClick(object sender, EventArgs e)
        {
            letterGuessed = "C";
            letterSelect();
            btnc.Enabled = false;
            btnc.Visibility = ViewStates.Invisible;
        }

        private void onBtnbClick(object sender, EventArgs e)
        {
            letterGuessed = "B";
            letterSelect();  
            btnb.Enabled = false;
            btnb.Visibility = ViewStates.Invisible;
        }

        private void onBtnaClick(object sender, EventArgs e)
        {
            letterGuessed = "A";
            letterSelect(); 
            btna.Enabled = false;
            btna.Visibility = ViewStates.Invisible;
        }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(MyActivity));
            intent.SetFlags(ActivityFlags.ClearTop);
            StartActivity(intent);
        }

        //not used remove Jaimie!
        void lstToDoList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, myList[e.Position].WORD, ToastLength.Long);
        }
        public void CopyDatabase()
        {
            if (!File.Exists(dbPath))
            {
                using (BinaryReader br = new BinaryReader(Assets.Open(dbName)))
                {
                    using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int len = 0;
                        while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, len);
                        }
                    }
                }
            }
        }//CopyDatabase() end
    }
}

