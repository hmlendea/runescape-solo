using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RuneScapeSolo.Net.Client.Data;
using RuneScapeSolo.Net.Client.Game;

namespace RuneScapeSolo.Net.Client
{
    public class GameApplet : IDisposable
    {
        public GameApplet()
        {
            // This is needed
        }

        public GameApplet(GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            InitGameApplet();
        }

        public void CreateWindow(int width, int height, string title, bool resizable)
        {
            Console.WriteLine("Started application");
            appletWidth = width;
            appletHeight = height;
            gameFrame = new GameFrame(this, width, height, title, resizable, false);
            gameLoadingScreen = 1;

            InitGameApplet();

            // gameWindowThread = new Thread(this);
            //  gameWindowThread.start();
            //  gameWindowThread.setPriority(1);
        }

        public virtual void LoadGame()
        {
        }

        public virtual void CheckInputs()
        {
        }

        public virtual void Close()
        {
        }

        public void SetRefreshRate(int i)
        {
            refreshRate = 1000 / i;
        }

        public void ResetTimings()
        {
            for (int i = 0; i < 10; i++)
            {
                timeArray[i] = 0L;
            }
        }

        public void MouseEntered(MouseState evt)
        {
            MouseMove(evt.X, evt.Y);
        }

        public void MouseExited(MouseState evt)
        {
            MouseMove(evt.X, evt.Y);
        }

        public void MousePressed(MouseState evt)
        {
            mouseDown(evt.X, evt.Y, evt.RightButton == ButtonState.Pressed);
        }

        public void MouseReleased(MouseState evt)
        {
            MouseUp(evt.X, evt.Y);
        }

        public void MouseDragged(MouseState evt)
        {
            mouseDrag(evt.Y, evt.X, evt.RightButton == ButtonState.Pressed);
        }

        public void MouseMoved(MouseState evt)
        {
            MouseMove(evt.X, evt.Y);
        }

        public void KeyDown(Keys key, char c)
        {
            HandleKeyDown(key, c);

            bool flag = false;

            for (int i = 0; i < allowedChars.Length; i++)
            {
                if (c != allowedChars[i] && key != Keys.Left && key != Keys.Right && key != Keys.Up && key != Keys.Down)
                {
                    continue;
                }

                flag = true;
                break;
            }

            if (flag && inputText.Length < 20)
            {
                inputText += c;
            }
            if (flag && privateMessageText.Length < 80)
            {
                privateMessageText += c;
            }
            if (key == Keys.Back && inputText.Length > 0)
            {
                inputText = inputText.Substring(0, inputText.Length - 1);
            }
            if (key == Keys.Back && privateMessageText.Length > 0)
            {
                privateMessageText = privateMessageText.Substring(0, privateMessageText.Length - 1);
            }
            if (key == Keys.Enter)
            {
                enteredInputText = inputText;
                enteredPrivateMessageText = privateMessageText;
            }
        }

        public virtual void HandleKeyDown(Keys key, char c)
        {
        }

        public void KeyUp(Keys key, char c)
        {
        }

        public bool MouseMove(int x, int y)
        {
            mouseButton = 0;
            return true;
        }

        public bool MouseUp(int x, int y)
        {
            mouseButton = 0;
            return true;
        }

        public bool mouseDown(int x, int y, bool metaDown)
        {
            mouseButton = metaDown ? 2 : 1;
            lastMouseButton = mouseButton;
            handleMouseDown(mouseButton, x, y);
            return true;
        }

        public virtual void handleMouseDown(int i, int k, int l)
        {
        }

        public bool mouseDrag(int x, int y, bool metaDown)
        {
            mouseButton = metaDown ? 2 : 1;
            return true;
        }

        public void Initialise()
        {
            Console.WriteLine("Started applet");

            appletWidth = 512;
            appletHeight = 344;
            gameLoadingScreen = 1;

            DataOperations.codeBase = default(Uri);
            //startThread(this);
        }

        public void StartThread(Action runnable)
        {
            ThreadStart threadStart = new ThreadStart(runnable);
            Thread thread = new Thread(threadStart);

            thread.Start();
        }

        public void Start()
        {
            if (runStatus >= 0)
            {
                runStatus = 0;
            }
        }

        public void Stop()
        {
            if (runStatus >= 0)
            {
                runStatus = 4000 / refreshRate;
            }
        }

        public void Dispose()
        {
            runStatus = -1;

            Thread.Sleep(2000);

            if (runStatus == -1)
            {
                Console.WriteLine("2 seconds expired, forcing kill");

                CloseProgram();

                if (gameWindowThread != null)
                {
                    gameWindowThread.Abort();
                    gameWindowThread = null;
                }
            }
        }

        public void CloseProgram()
        {
            runStatus = -2;
            Console.WriteLine("Closing program");

            Close();
        }

        //Component getGameComponent() {
        //    if(gameFrame != null)
        //        return gameFrame;
        //    else
        //        return this;
        //}

        public void LoadApp()
        {

        }

        public void run()
        {
            //getGameComponent().addKeyListener(this);
            //getGameComponent().addMouseListener(this);
            //getGameComponent().addMouseMotionListener(this);


            if (gameLoadingScreen == 1)
            {
                gameLoadingScreen = 2;
                loadLoadingScreen();
                drawLoadingScreen(0, "Loading...");
                LoadGame();
                gameLoadingScreen = 0;
            }

            for (int k1 = 0; k1 < 10; k1++)
            {
                timeArray[k1] = CurrentTimeMillis();
            }

            while (runStatus >= 0)
            {
                UpdateGame(gameVar_i, gameVar_k, gameVar_sleepTime, gameVar_j1);
                OnDrawDone();
            }
            if (runStatus == -1)
            {
                CloseProgram();
                gameWindowThread = null;
            }
        }

        public bool DrawIsNecessary;

        public void OnDrawDone()
        {
            DrawIsNecessary = true;
        }

        public int gameVar_i = 0;
        public int gameVar_k = 256;
        public int gameVar_sleepTime = 1;
        public int gameVar_j1 = 0;

        public void UpdateGame(int i, int k, int sleepTime, int j1)
        {
            if (runStatus > 0)
            {
                runStatus--;
                if (runStatus == 0)
                {
                    CloseProgram();
                    gameWindowThread = null;
                    return;
                }
            }
            int i2 = k;
            int j2 = sleepTime;
            k = 300;
            sleepTime = 1;
            long l1 = CurrentTimeMillis();//System.currentTimeMillis();
            if (timeArray[i] == 0L)
            {
                k = i2;
                sleepTime = j2;
            }
            else if (l1 > timeArray[i])
            {
                k = (int)(2560 * refreshRate / (l1 - timeArray[i]));
            }

            if (k < 25)
            {
                k = 25;
            }

            if (k > 256)
            {
                k = 256;
                sleepTime = (int)(refreshRate - (l1 - timeArray[i]) / 10L);
                if (sleepTime < gameMinThreadSleepTime)
                {
                    sleepTime = gameMinThreadSleepTime;
                }
            }

            Thread.Sleep(sleepTime);

            timeArray[i] = l1;
            i = (i + 1) % 10;
            if (sleepTime > 1)
            {
                for (int k2 = 0; k2 < 10; k2++)
                {
                    if (timeArray[k2] != 0L)
                    {
                        timeArray[k2] += sleepTime;
                    }
                }
            }
            int l2 = 0;
            while (j1 < 256)
            {
                var start = DateTime.Now;
                CheckInputs();
                j1 += k;
                if (++l2 > fie)
                {
                    j1 = 0;
                    fij += 6;
                    if (fij > 25)
                    {
                        fij = 0;
                    }
                    break;
                }
                var end = DateTime.Now - start;
            }
            fij--;
            j1 &= 0xff;
            //drawWindow();
            // paint(graphics);
        }

        public virtual void DrawWindow()
        {

        }

        public void paint(GraphicsDevice g1)
        {
            if (gameLoadingScreen == 2)
            {
                drawLoadingScreen(gameLoadingPercentage, gameLoadingFileTitle);
                return;
            }
        }

        void loadLoadingScreen()
        {
            sbyte[] bytes = unpackData("fonts.jag", "Game fonts", 0);

            GameImage.addFont(DataOperations.loadData("h11p.jf", 0, bytes));
            GameImage.addFont(DataOperations.loadData("h12b.jf", 0, bytes));
            GameImage.addFont(DataOperations.loadData("h12p.jf", 0, bytes));
            GameImage.addFont(DataOperations.loadData("h13b.jf", 0, bytes));
            GameImage.addFont(DataOperations.loadData("h14b.jf", 0, bytes));
            GameImage.addFont(DataOperations.loadData("h16b.jf", 0, bytes));
            GameImage.addFont(DataOperations.loadData("h20b.jf", 0, bytes));
            GameImage.addFont(DataOperations.loadData("h24b.jf", 0, bytes));
        }

        void drawLoadingScreen(int percentage, string fileTitle)
        {
            try
            {
                int i = (appletWidth - 281) / 2;
                int k = (appletHeight - 148) / 2;
                //graphics.setColor(Color.Black);
                //graphics.fillRect(0, 0, appletWidth, appletHeight);
                //graphics.Clear(Color.Black);

                i += 2;
                k += 90;



                //if (bgImage != null)
                //{
                //    // spriteBatch.BeginSafe();
                //    spriteBatch.Draw(bgImage, Vector2.Zero, Color.White);
                //    // spriteBatch.EndSafe();
                //}
                //  graphics.drawImage(bgImage, 0, 0, null);
                gameLoadingPercentage = percentage;
                gameLoadingFileTitle = fileTitle;
                //graphics.setColor();

                //spriteBatch.drawRect(new Rectangle(i - 2, k - 2, 280, 23), new Color(132, 132, 132));
                //spriteBatch.fillRect(new Rectangle(i, k, (277 * percentage) / 100, 20), new Color(132, 132, 132));


                //graphics.setColor(new Color(198, 198, 198));
                //drawString(fileTitle/*, gameLoadingFont*/, i + 138, k + 10, new Color(198, 198, 198));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured in {nameof(GameApplet)}.cs");
                Console.WriteLine(ex.Message);
            }
        }

        public void drawLoadingBarText(int i, string s)
        {
            try
            {
                int k = (appletWidth - 281) / 2;
                int l = (appletHeight - 148) / 2;
                k += 2;
                l += 90;
                gameLoadingPercentage = i;
                gameLoadingFileTitle = s;
                int i1 = (277 * i) / 100;
                // spriteBatch.fillRect(new Rectangle(k, l, i1, 20), new Color(132, 132, 132));
                //  graphics.setColor(new Color(132, 132, 132));
                //  graphics.fillRect(k, l, i1, 20);
                //  graphics.setColor(Color.black);
                //  spriteBatch.fillRect(new Rectangle(k + i1, l, 277 - i1, 20), Color.Black);
                //graphics.fillRect(k + i1, l, 277 - i1, 20);
                //graphics.setColor(new Color(198, 198, 198));

                //drawString(graphics, s, gameLoadingFont, k + 138, l + 10, new Color(198, 198, 198));
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured in {nameof(GameApplet)}.cs");
                return;
            }
        }

        //public void drawString(string arg1, int arg3, int arg4, Color color)
        //{
        //    //Object obj;
        //    //if (gameFrame == null)
        //    //    obj = this;
        //    //else
        //    //    obj = gameFrame;
        //    //var fontmetrics = arg2.MeasureString(arg1);//((Component)(obj)).getFontMetrics(arg2);
        //    //fontmetrics.stringWidth(arg1);
        //    //arg0.setFont(arg2);
        //    //arg0.drawString(arg1, arg3 - fontmetrics.stringWidth(arg1) / 2, arg4 + fontmetrics.getHeight() / 4);

        //    //GameImage.stringsToDraw.Add(new stringDrawDef
        //    //{
        //    //    font = arg2,
        //    //    text = arg1,
        //    //    pos = new Vector2(arg3 - fontmetrics.X / 2, arg4 + fontmetrics.Y / 4),
        //    //    forecolor = color
        //    //});

        //    //spriteBatch.BeginSafe();
        //    //spriteBatch.DrawString(arg2, arg1, new Vector2(fontmetrics.X / 2, arg4 + fontmetrics.Y / 4), color);
        //    //spriteBatch.EndSafe();
        //}

        public virtual sbyte[] unpackData(string filename, string fileTitle, int startPercentage)
        {

            Console.WriteLine("Using default load");
            int i = 0;
            int k = 0;
            sbyte[] abyte0 = Link.getFile(filename);
            if (abyte0 == null)
            {
                try
                {
                    Console.WriteLine("Loading " + fileTitle + " - 0%");
                    drawLoadingBarText(startPercentage, "Loading " + fileTitle + " - 0%");
                    var inputstream = new BinaryReader(DataOperations.openInputStream(filename));
                    //DataInputStream datainputstream = new DataInputStream(inputstream);
                    sbyte[] abyte2 = new sbyte[6] {
                        inputstream.ReadSByte(),inputstream.ReadSByte(),inputstream.ReadSByte(),
                        inputstream.ReadSByte(),inputstream.ReadSByte(),inputstream.ReadSByte()
                    };

                    //inputstream.Read(abyte2, 0, 6);
                    i = ((abyte2[0] & 0xff) << 16) + ((abyte2[1] & 0xff) << 8) + (abyte2[2] & 0xff);
                    k = ((abyte2[3] & 0xff) << 16) + ((abyte2[4] & 0xff) << 8) + (abyte2[5] & 0xff);



                    Console.WriteLine("Loading " + fileTitle + " - 5%");
                    drawLoadingBarText(startPercentage, "Loading " + fileTitle + " - 5%");
#warning this could break stuff
                    // int l = 0;
                    int l = 6;
                    abyte0 = new sbyte[k];
                    while (l < k)
                    {
                        int i1 = k - l;
                        if (i1 > 1000)
                        {
                            i1 = 1000;
                        }

                        for (int t = 0; t < i1; t++)
                        {
                            abyte0[l + t] = inputstream.ReadSByte();
                        }

                        // inputstream.Read(abyte0, l, i1);

                        l += i1;
                        Console.WriteLine("Loading " + fileTitle + " - " + (5 + (l * 95) / k) + "%");
                        drawLoadingBarText(startPercentage, "Loading " + fileTitle + " - " + (5 + (l * 95) / k) + "%");
                    }

                    inputstream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error has occured in {nameof(GameApplet)}.cs");
                }
            }

            Console.WriteLine("Unpacking " + fileTitle);
            drawLoadingBarText(startPercentage, "Unpacking " + fileTitle);
            if (k != i)
            {
                sbyte[] abyte1 = new sbyte[i];
                DataFileDecrypter.unpackData(abyte1, i, abyte0, k, 0);
                return abyte1;
            }
            else
            {
                //  return unpackData(filename, fileTitle, startPercentage); // abyte0;
                return abyte0;
            }
        }

        //public Texture2D createImage(int i, int k)
        //{
        //    //if (gameFrame != null)
        //    //    return gameFrame.createImage(i, k);
        //    //else
        //    //    return super.createImage(i, k);

        //    return new Texture2D(this.graphics, i, k);
        //}

        protected TcpClient MakeSocket(string ip, int port)
        {
            TcpClient client = new TcpClient();
            client.Connect(ip, port);

            client.SendTimeout = 30000;
            client.NoDelay = true;

            return client;
        }

        public void InitGameApplet()
        {
            appletWidth = 512;
            appletHeight = 384;
            refreshRate = 60;
            fie = 1000;
            timeArray = new long[10];
            gameLoadingScreen = 1;
            gameLoadingFileTitle = "Loading";
            //gameLoadingFont = loadingFont;//new Font("TimesRoman", 0, 15);
            gameMinThreadSleepTime = 1;
            inputText = "";
            enteredInputText = "";
            privateMessageText = "";
            enteredPrivateMessageText = "";
        }

        static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        //GameApplet baseApplet;
        int appletWidth;
        int appletHeight;
        public Thread gameWindowThread;
        int refreshRate;
        int fie;
        long[] timeArray;
        public static GameFrame gameFrame = null;
        public int runStatus;
        public int fij;
        public int gameLoadingScreen;
        public int gameLoadingPercentage;
        public string gameLoadingFileTitle;
        public static string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö0123456789!\"!$%^&*()-_=+[{]};:'@#~,<.>/?\\| ";
        public int gameMinThreadSleepTime;
        public int mouseButton;
        public int lastMouseButton;
        public string inputText;
        public string enteredInputText;
        public string privateMessageText;
        public string enteredPrivateMessageText;

        public static int[][] bgPixels = null;
        public static Texture2D bgImage = null;
    }
}