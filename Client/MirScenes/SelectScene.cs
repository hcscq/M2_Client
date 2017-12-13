using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using C = ClientPackets;
using S = ServerPackets;
using System.Threading;
namespace Client.MirScenes
{
    public class SelectScene : MirScene
    {
        public MirImageControl Background, Title;
        private NewCharacterDialog _character;

        public MirLabel ServerLabel;
        public MirAnimatedControl[] CharacterDisplay;
        public MirButton StartGameButton, NewCharacterButton, DeleteCharacterButton, CreditsButton, ExitGame;
        public CharacterControl[] CharacterControls;
        public MirLabel LastAccessLabel, LastAccessLabelLabel;
        public List<SelectInfo> Characters = new List<SelectInfo>();
        private byte _selected;

        public SelectScene(List<SelectInfo> characters)
        {
            SoundManager.PlaySound(SoundList.SelectMusic, true);
            Disposing += (o, e) => SoundManager.StopSound(SoundList.SelectMusic);
            CharacterDisplay = new MirAnimatedControl[2+1];

            Characters = characters;
            SortList();
            if(Characters.Count>0)
                _selected =Characters[Characters.Count-1].Index;
            KeyPress += SelectScene_KeyPress;

            Background = new MirImageControl
            {
                Index = 65,
                Library = Libraries.Prguse,
                Parent = this,
            };

            Title = new MirImageControl
            {
                Index = 40,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(364, 12)
            };

            ServerLabel = new MirLabel
            {
                Location = new Point(322, 5),
                Parent = Background,
                Size = new Size(155, 16),
                Text = g_ServerName,//"Legend of Mir 2",
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            };

            StartGameButton = new MirButton
            {
                TakeSizeMode = UsedSize.HoverIndex,
                Enabled = false,
                HoverIndex = 68,//341,
                //Index = 68,//340,
                Library = Libraries.Prguse,//Libraries.Title,
                Location = new Point(385,455),//new Point(110, 568),
                Parent = Background,
                PressedIndex = 68,//342,
                GrayScale = true

            };
            StartGameButton.Click += (o, e) => StartGame();



            DeleteCharacterButton = new MirButton
            {
                TakeSizeMode = UsedSize.HoverIndex,
                HoverIndex = 70,// 347,
                //Index = 70,//346,
                Library = Libraries.Prguse,//Libraries.Title,
                Location = new Point(350, 505),
                Parent = Background,
                PressedIndex = 70,//348
            };
            DeleteCharacterButton.Click += (o, e) => DeleteCharacter();


            CreditsButton = new MirButton
            {
                TakeSizeMode = UsedSize.HoverIndex,
                HoverIndex = 71,//350,
                //Index = 71,//349,
                Library = Libraries.Prguse,//Libraries.Title,
                Location = new Point(363, 526),
                Parent = Background,
                PressedIndex = 71,//351

            };

            ExitGame = new MirButton
            {
                TakeSizeMode = UsedSize.HoverIndex,
                HoverIndex = 72,//353,
                //Index = 72,//352,
                Library = Libraries.Prguse,//Libraries.Title,
                Location = new Point(380, 546),
                Parent = Background,
                PressedIndex = 72,//354

            };
            ExitGame.Click += (o, e) => Program.Form.Close();

            #region Animations

            CharacterDisplay = new MirAnimatedControl[3];
            //CharacterDisplay[0].Location = new Point(120,100);
            //CharacterDisplay[1].Location = new Point(500,100);
            //CharacterDisplay[2].Location = new Point(120, 100);
            #endregion

            CharacterControls = new CharacterControl[4];

            CharacterControls[0] = new CharacterControl(Background){CharIndex = 0};
            CharacterControls[0].SelectButton.Location = new Point(134,453);
            CharacterControls[0].CharacterDisplay.Location = new Point(60,50);
            CharacterControls[0].CharacterDisplay.Border = true;
            CharacterControls[0].CharacterDisplay.BorderColour = Color.Red;
            CharacterControls[0].NameLabel.Location = new Point(110,492);
            CharacterControls[0].LevelLabel.Location = new Point(110, 522);
            CharacterControls[0].ClassLabel.Location = new Point(110, 552);
            CharacterControls[0].SelectButton.Sound = SoundList.ButtonA;

            CharacterControls[0].SelectButton.Click += (o, e) =>
            {
                if (characters.Count <= 0) return;

                _selected = CharacterControls[0].CharIndex;
                UpdateInterface();
            };

            CharacterControls[1] = new CharacterControl(Background) { CharIndex = 1 };
            CharacterControls[1].SelectButton.Location = new Point(685,453);
            CharacterControls[1].CharacterDisplay.Location = new Point(450,25);
            CharacterControls[1].NameLabel.Location = new Point(665,493);
            CharacterControls[1].LevelLabel.Location = new Point(665, 523);
            CharacterControls[1].ClassLabel.Location = new Point(665, 553);
            CharacterControls[1].SelectButton.Sound = SoundList.ButtonA;

            CharacterControls[1].SelectButton.Click += (o, e) =>
            {
                if (characters.Count <= 1) return;
                _selected = CharacterControls[1].CharIndex;  
                UpdateInterface();
            };

            #region 2,3 characters
            //CharacterControls[2] = new CharacterControl
            //{
            //    Location = new Point(447, 330),
            //    Parent = Background,
            //    Sound = SoundList.ButtonA,
            //};
            //CharacterControls[2].Click += (o, e) =>
            //{
            //    if (characters.Count <= 2) return;

            //    _selected = 2;
            //    UpdateInterface();
            //};

            //CharacterControls[3] = new CharacterControl
            //{
            //    Location = new Point(447, 434),
            //    Parent = Background,
            //    Sound = SoundList.ButtonA,
            //};
            //CharacterControls[3].Click += (o, e) =>
            //{
            //    if (characters.Count <= 3) return;

            //    _selected = 3;
            //    UpdateInterface();
            //};
            #endregion
            NewCharacterButton = new MirButton
            {
                HoverIndex = 69,//344,
                //Index = 69,//343,
                Library = Libraries.Prguse,//Libraries.Title,
                Location = new Point(350, 484),//new Point(230, 568),
                Parent = Background,
                PressedIndex = 69,//345,
                TakeSizeMode = UsedSize.HoverIndex
            };
            NewCharacterButton.Click += (o, e) =>
            {
                for (int i = 0; i < CharacterControls.Length; i++)
                    if (CharacterControls[i].IsEmpty)
                    {
                        _character = new NewCharacterDialog
                        {
                            CharIndex = CharacterControls[i].CharIndex,
                            Parent = Background,
                            Location = CharacterControls[i > 0 ? (i - 1) : (i + 1)].CharacterDisplay.Location,
                            CharacterDisplay = CharacterControls[i].CharacterDisplay

                        };
                        return;
                    }
            };
            LastAccessLabel = new MirLabel
            {
                Location = new Point(140, 509),
                Parent = Background,
                Size = new Size(180, 21),
                DrawFormat = TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                Border = true,
            };
            LastAccessLabelLabel = new MirLabel
            {
                Location = new Point(-80, -1),
                Parent = LastAccessLabel,
                Text = "Last Online:",
                Size = new Size(100, 21),
                DrawFormat = TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                Border = true,
            };
            UpdateInterface();
        }

        private void SelectScene_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            if (StartGameButton.Enabled)
                StartGame();
            e.Handled = true;
        }


        public void SortList()
        {
            if (Characters != null)
                Characters.Sort((c1, c2) => c2.LastAccess.CompareTo(c1.LastAccess));
        }


        public void StartGame()
        {
            if (!Libraries.Loaded)
            {
                MirMessageBox message = new MirMessageBox(string.Format("Please wait, The game is still loading... {0:##0}%", Libraries.Progress / (double)Libraries.Count * 100), MirMessageBoxButtons.Cancel);

                message.BeforeDraw += (o, e) => message.Label.Text = string.Format("Please wait, The game is still loading... {0:##0}%", Libraries.Progress / (double)Libraries.Count * 100);

                message.AfterDraw += (o, e) =>
                {
                    if (!Libraries.Loaded) return;
                    message.Dispose();
                    StartGame();
                };

                message.Show();

                return;
            }
            StartGameButton.Enabled = false;

            Network.Enqueue(new C.StartGame
            {
                CharacterIndex = Characters[_selected].Index,
                Account=g_Account
            });
        }

        public override void Process()
        {
            

        }
        public override void ProcessPacket(Packet p)
        {
            switch (p.Index)
            {
                case (short)ServerPacketIds.SM_NEWCHR_FAIL:
                    NewCharacter((S.NewCharacter)p);
                    break;
                case (short)ServerPacketIds.SM_NEWCHR_SUCCESS:
                    NewCharacter((S.NewCharacterSuccess)p);
                    break;
                case (short)ServerPacketIds.SM_DELCHR_FAIL:
                    DeleteCharacter((S.DeleteCharacter)p);
                    break;
                case (short)ServerPacketIds.SM_DELCHR_SUCCESS:
                    DeleteCharacter((S.DeleteCharacterSuccess)p);
                    break;
                case (short)ServerPacketIds.SM_STARTPLAY:
                    StartGame((S.StartGame)p);
                    break;
                case (short)ServerPacketIds.StartGameBanned:
                    StartGame((S.StartGameBanned)p);
                    break;
                case (short)ServerPacketIds.StartGameDelay:
                    StartGame((S.StartGameDelay) p);
                    break;
                default:
                    base.ProcessPacket(p);
                    break;
            }
        }

        private void NewCharacter(S.NewCharacter p)
        {
            _character.OKButton.Enabled = true;
            
            switch (p.Result)
            {
                case 0:
                    MirMessageBox.Show("Creating new characters is currently disabled.");
                    _character.Dispose();
                    break;
                case 1:
                    MirMessageBox.Show("Your Character Name is not acceptable.");
                    _character.NameTextBox.SetFocus();
                    break;
                case 2:
                    MirMessageBox.Show("The gender you selected does not exist.\n Contact a GM for assistance.");
                    break;
                case 3:
                    MirMessageBox.Show("The class you selected does not exist.\n Contact a GM for assistance.");
                    break;
                case 4:
                    MirMessageBox.Show("You cannot make anymore then " + Globals.MaxCharacterCount + " Characters.");
                    _character.Dispose();
                    break;
                case 5:
                    MirMessageBox.Show("A Character with this name already exists.");
                    _character.NameTextBox.SetFocus();
                    break;
            }


        }
        private void NewCharacter(S.NewCharacterSuccess p)
        {
            _character.Dispose();
            MirMessageBox.Show("Your character was created successfully.");
            
            Characters.Insert(0, p.CharInfo);
            _selected = p.CharInfo.Index;
            UpdateInterface();
        }

        private void DeleteCharacter()
        {
            if (_selected < 0 || _selected >= Characters.Count) return;

            MirMessageBox message = new MirMessageBox(string.Format("Are you sure you want to Delete the character {0}?", new string(Characters[_selected].Name)), MirMessageBoxButtons.YesNo);
            byte index = _selected;//Characters[_selected].Index;

            message.YesButton.Click += (o, e) =>
            {
                DeleteCharacterButton.Enabled = false;
                Network.Enqueue(new C.DeleteCharacter { CharacterIndex = index,Account=g_Account });
            };

            message.Show();
        }

        private void DeleteCharacter(S.DeleteCharacter p)
        {
            short SUCCESS = 16;
            byte CHARACTERMASK = 15;
            DeleteCharacterButton.Enabled = true;
            if ((p.wParam & SUCCESS) <= 0)
                switch (p.wParam)
                {
                    case 0:
                        MirMessageBox.Show("Deleting characters is currently disabled.");
                        break;
                    case 1:
                        MirMessageBox.Show("The character you selected does not exist.\n Contact a GM for assistance.");
                        break;
                }
            else {
                DeleteCharacterButton.Enabled = true;
                MirMessageBox.Show("Your character was deleted successfully.");
                byte charIndex = (byte)(CHARACTERMASK & p.wParam);
                for (int i = 0; i < Characters.Count; i++)
                    if (Characters[i].Index == charIndex)
                    {
                        Characters.RemoveAt(i);
                        break;
                    }
                if (Characters.Count > 0)
                    _selected = Characters[0].Index;
                UpdateInterface();
            }
        }
        private void DeleteCharacter(S.DeleteCharacterSuccess p)
        {
            DeleteCharacterButton.Enabled = true;
            MirMessageBox.Show("Your character was deleted successfully.");

            for (int i = 0; i < Characters.Count; i++)
                if (Characters[i].Index == p.CharacterIndex)
                {
                    Characters.RemoveAt(i);
                    break;
                }
            if (Characters.Count > 0)
                _selected = Characters[0].Index;
            UpdateInterface();
        }

        private void StartGame(S.StartGameDelay p)
        {
            StartGameButton.Enabled = true;

            long time = CMain.Time + p.Milliseconds;

            MirMessageBox message = new MirMessageBox(string.Format("You cannot log onto this character for another {0} seconds.", Math.Ceiling(p.Milliseconds/1000M)));

            message.BeforeDraw += (o, e) => message.Label.Text = string.Format("You cannot log onto this character for another {0} seconds.", Math.Ceiling((time - CMain.Time)/1000M));
                

            message.AfterDraw += (o, e) =>
            {
                if (CMain.Time <= time) return;
                message.Dispose();
                StartGame();
            };

            message.Show();
        }
        public void StartGame(S.StartGameBanned p)
        {
            StartGameButton.Enabled = true;

            TimeSpan d = p.ExpiryDate - CMain.Now;
            MirMessageBox.Show(string.Format("This account is banned.\n\nReason: {0}\nExpiryDate: {1}\nDuration: {2:#,##0} Hours, {3} Minutes, {4} Seconds", p.Reason,
                                             p.ExpiryDate, Math.Floor(d.TotalHours), d.Minutes, d.Seconds));
        }
        public void StartGame(S.StartGame p)
        {
            StartGameButton.Enabled = true;
            //2017.06.20
            //if (p.Resolution < Settings.Resolution || Settings.Resolution == 0) Settings.Resolution = p.Resolution;

            //if (p.Resolution < 1024 || Settings.Resolution < 1024) Settings.Resolution = 800;
            //else if (p.Resolution < 1366 || Settings.Resolution < 1280) Settings.Resolution = 1024;
            //else if (p.Resolution < 1366 || Settings.Resolution < 1366) Settings.Resolution = 1280;//not adding an extra setting for 1280 on server cause well it just depends on the aspect ratio of your screen
            //else if (p.Resolution >= 1366 && Settings.Resolution >= 1366) Settings.Resolution = 1366;

            switch (p.wParam)
            {
                case 0:
                    MirMessageBox.Show("Starting the game is currently disabled.");
                    break;
                case 1:
                    MirMessageBox.Show("You are not logged in.");
                    break;
                case 2:
                    MirMessageBox.Show("Your character could not be found.");
                    break;
                case 3:
                    MirMessageBox.Show("No active map and/or start point found.");
                    break;
                case 4:
                    //2017.06.20
                    //if (Settings.Resolution == 1024)
                    //    CMain.SetResolution(1024, 768);
                    //else if (Settings.Resolution == 1280)
                    //    CMain.SetResolution(1280, 800);
                    //else if (Settings.Resolution == 1366)
                    //    CMain.SetResolution(1366, 768);
                    string []ipInfo = System.Text.Encoding.Default.GetString(p.ServerIP).Replace('\0',' ').Trim().Split('/');
                    if (ipInfo.Length < 2)
                    {
                        MirMessageBox.Show("Get server error.");
                        return;
                    }
                    Network.ConnectionChangeTo(ipInfo[0],ipInfo[1],
                        new C.Certification
                        {
                            Account = g_Account,
                            CharIndex = Characters[_selected].Index,
                            nCertification = g_nCertifacation,
                            StarNew = 1
                        }
                        );
                    ActiveScene = new GameScene();
                    Dispose();
                    break;
            }
        }
        private void UpdateInterface()
        {
            for (int i = 0; i < CharacterControls.Length; i++)
            {
                if (CharacterControls[i] == null) continue;
                CharacterControls[i].Selected = CharacterControls[i].CharIndex == _selected;
                if (CharacterControls[i].Selected)
                    StartGameButton.Enabled = true;
                CharacterControls[i].Update((Characters.Exists(it => it.Index == CharacterControls[i].CharIndex)) ? Characters.Find(it => it.Index == CharacterControls[i].CharIndex) : null);
            }
            #region shared stage
            //if (_selected >= 0 && _selected < Characters.Count)
            //{
            //    CharacterDisplay.Visible = true;
            //    //CharacterDisplay.Index = ((byte)Characters[_selected].Class + 1) * 20 + (byte)Characters[_selected].Gender * 280; 

            //    switch ((MirClass)Characters[_selected].Class)
            //    {
            //        case MirClass.Warrior:
            //            CharacterDisplay.Index = (byte)Characters[_selected].Gender == 0 ? 20 : 300; //220 : 500;
            //            break;
            //        case MirClass.Wizard:
            //            CharacterDisplay.Index = (byte)Characters[_selected].Gender == 0 ? 40 : 320; //240 : 520;
            //            break;
            //        case MirClass.Taoist:
            //            CharacterDisplay.Index = (byte)Characters[_selected].Gender == 0 ? 60 : 340; //260 : 540;
            //            break;
            //        case MirClass.Assassin:
            //            CharacterDisplay.Index = (byte)Characters[_selected].Gender == 0 ? 80 : 360; //280 : 560;
            //            break;
            //        case MirClass.Archer:
            //            CharacterDisplay.Index = (byte)Characters[_selected].Gender == 0 ? 100 : 140; //160 : 180;
            //            break;
            //    }

            //    LastAccessLabel.Text = Characters[_selected].LastAccess == DateTime.MinValue ? "Never" : Characters[_selected].LastAccess.ToString();
            //    LastAccessLabel.Visible = true;
            //    LastAccessLabelLabel.Visible = true;
            //    StartGameButton.Enabled = true;
            //}
            //else
            //{
            //    CharacterDisplay.Visible = false;
            //    LastAccessLabel.Visible = false;
            //    LastAccessLabelLabel.Visible = false;
            //    StartGameButton.Enabled = false;
            //}
            #endregion
        }


        #region Disposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Background = null;
                _character = null;

                ServerLabel = null;
                CharacterDisplay = null;
                StartGameButton = null;
                NewCharacterButton = null;
                DeleteCharacterButton = null; 
                CreditsButton = null;
                ExitGame = null;
                CharacterControls = null;
                LastAccessLabel = null;LastAccessLabelLabel = null;
                Characters  = null;
                _selected = 0;
            }

            base.Dispose(disposing);
        }
        #endregion
        public sealed class NewCharacterDialog : MirImageControl
        {
            private static readonly Regex Reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinCharacterNameLength + "," + Globals.MaxCharacterNameLength + "}$");

            //public MirImageControl TitleLabel;
            public MirAnimatedControl CharacterDisplay;

            public MirButton OKButton,
                             CancelButton,
                             WarriorButton,
                             WizardButton,
                             TaoistButton,
                             AssassinButton,
                             ArcherButton,
                             MaleButton,
                             FemaleButton;

            public MirTextBox NameTextBox;

            public MirLabel Description;

            private MirClass _class;
            private MirGender _gender;
            public byte CharIndex;
            #region Descriptions
            public const string WarriorDescription =
                "Warriors are a class of great strength and vitality. They are not easily killed in battle and have the advantage of being able to use" +
                " a variety of heavy weapons and Armour. Therefore, Warriors favor attacks that are based on melee physical damage. They are weak in ranged" +
                " attacks, however the variety of equipment that are developed specifically for Warriors complement their weakness in ranged combat.";

            public const string WizardDescription =
                "Wizards are a class of low strength and stamina, but have the ability to use powerful spells. Their offensive spells are very effective, but" +
                " because it takes time to cast these spells, they're likely to leave themselves open for enemy's attacks. Therefore, the physically weak wizards" +
                " must aim to attack their enemies from a safe distance.";

            public const string TaoistDescription =
                "Taoists are well disciplined in the study of Astronomy, Medicine, and others aside from Mu-Gong. Rather then directly engaging the enemies, their" +
                " specialty lies in assisting their allies with support. Taoists can summon powerful creatures and have a high resistance to magic, and is a class" +
                " with well balanced offensive and defensive abilities.";

            public const string AssassinDescription =
                "Assassins are members of a secret organization and their history is relatively unknown. They're capable of hiding themselves and performing attacks" +
                " while being unseen by others, which naturally makes them excellent at making fast kills. It is necessary for them to avoid being in battles with" +
                " multiple enemies due to their weak vitality and strength.";

            public const string ArcherDescription = 
                "Archers are a class of great accuracy and strength, using their powerful skills with bows to deal extraordinary damage from range. Much like" +
                " wizards, they rely on their keen instincts to dodge oncoming attacks as they tend to leave themselves open to frontal attacks. However, their" +
                " physical prowess and deadly aim allows them to instil fear into anyone they hit.";

            #endregion

            public NewCharacterDialog()
            {
                Index = 73;
                Library = Libraries.Prguse;
                //Location = new Point((Settings.ScreenWidth - Size.Width)/2, (Settings.ScreenHeight - Size.Height)/2);
                Modal = true;

                //TitleLabel = new MirImageControl
                //    {
                //        Index = 20,
                //        Library = Libraries.Title,
                //        Location = new Point(206, 11),
                //        Parent = this,
                //    };

                _gender = MirGender.Male;
                _class = MirClass.Wizard;
                CancelButton = new MirButton
                    {
                        HoverIndex =11, //281,
                        Index = 11,//280,
                        Library = Libraries.Title,
                        Location = new Point(247,29),//new Point(425, 425),
                        Parent = this,
                        PressedIndex = 10//282
                    };
                CancelButton.Click += (o, e) => Dispose();


                OKButton = new MirButton
                    {
                        Enabled = false,
                        HoverIndex = 15,//361,
                        Index = 15,//360,
                        Library = Libraries.Title,
                        Location = new Point(102,360),//new Point(160, 425),
                        Parent = this,
                        PressedIndex = 14,//362,
                    };
                OKButton.Click += (o, e) => CreateCharacter();

                NameTextBox = new MirTextBox
                {
                    Location = new Point(70, 106),//new Point(325, 268),
                    Parent = this,
                    Size = new Size(135, 20),
                    MaxLength = Globals.MaxCharacterNameLength
                };
                NameTextBox.TextBox.KeyPress += TextBox_KeyPress;
                NameTextBox.TextBox.TextChanged += CharacterNameTextBox_TextChanged;
                NameTextBox.SetFocus();

                //CharacterDisplay = new MirAnimatedControl
                //    {
                //        Animated = true,
                //        AnimationCount = 16,
                //        AnimationDelay = 250,
                //        Index = 40,//20,
                //        Library = Libraries.ChrSel,
                //        Location =new Point(120,100),// new Point(120, 250),
                //        Parent = this,
                //        UseOffSet = true,
                //    };
                //CharacterDisplay.AfterDraw += (o, e) =>
                //    {
                //        if (_class == MirClass.Wizard)
                //            Libraries.ChrSel.DrawBlend(CharacterDisplay.Index + 560, CharacterDisplay.DisplayLocationWithoutOffSet, Color.White, true);
                //    };

                //74,75,76 55,56,57
                WarriorButton = new MirButton
                    {
                        HoverIndex = 74,//2427,
                        Index = 74,//2427,
                        Library = Libraries.Prguse,
                        Location = new Point(47, 156),
                        Parent = this,
                        PressedIndex = 55,//2428,
                        Sound = SoundList.ButtonA,
                    };
                WarriorButton.Click += (o, e) =>
                    {
                        _class = MirClass.Warrior;
                        UpdateInterface();
                    };


                WizardButton = new MirButton
                {
                    HoverIndex = 75,//2430,
                    Index = 75,//2429,
                    Library = Libraries.Prguse,
                    Location = new Point(92, 156),
                    Parent = this,
                    PressedIndex = 56,//2431,
                    Sound = SoundList.ButtonA,
                };
                WizardButton.Click += (o, e) =>
                    {
                        _class = MirClass.Wizard;
                        UpdateInterface();
                    };


                TaoistButton = new MirButton//1
                    {
                        HoverIndex = 76,//2433,
                        Index = 76,//2432,
                        Library = Libraries.Prguse,
                        Location = new Point(137, 156),
                        Parent = this,
                        PressedIndex = 57,//2434,
                        Sound = SoundList.ButtonA,
                    };
                TaoistButton.Click += (o, e) =>
                    {
                        _class = MirClass.Taoist;
                        UpdateInterface();
                    };
                #region two new career
                //AssassinButton = new MirButton
                //    {
                //        HoverIndex = 2436,
                //        Index = 2435,
                //        Library = Libraries.Prguse,
                //        Location = new Point(473, 296),
                //        Parent = this,
                //        PressedIndex = 2437,
                //        Sound = SoundList.ButtonA,
                //    };
                //AssassinButton.Click += (o, e) =>
                //    {
                //        _class = MirClass.Assassin;
                //        UpdateInterface();
                //    };

                //ArcherButton = new MirButton
                //{
                //    HoverIndex = 2439,
                //    Index = 2438,
                //    Library = Libraries.Prguse,
                //    Location = new Point(523, 296),
                //    Parent = this,
                //    PressedIndex = 2440,
                //    Sound = SoundList.ButtonA,
                //};
                //ArcherButton.Click += (o, e) =>
                //{
                //    _class = MirClass.Archer;
                //    UpdateInterface();
                //};
                #endregion
                //58 m,59 f 77,78
                MaleButton = new MirButton
                    {
                        HoverIndex = 77,//2421,
                        Index = 77,//2421,
                        Library = Libraries.Prguse,
                        Location = new Point(92, 231),
                        Parent = this,
                        PressedIndex = 58,//2422,
                        Sound = SoundList.ButtonA,
                    };
                MaleButton.Click += (o, e) =>
                    {
                        _gender = MirGender.Male;
                        UpdateInterface();
                    };

                FemaleButton = new MirButton
                    {
                        HoverIndex = 78,//2424,
                        Index = 78,//2423,
                        Library = Libraries.Prguse,
                        Location = new Point(137, 231),
                        Parent = this,
                        PressedIndex = 59,//2425,
                        Sound = SoundList.ButtonA,
                    };
                FemaleButton.Click += (o, e) =>
                    {
                        _gender = MirGender.Female;
                        UpdateInterface();
                    };

                Description = new MirLabel
                    {
                        Border = true,
                        Location = new Point(279, 70),
                        Parent = this,
                        Size = new Size(278, 170),
                        Text = WarriorDescription,
                    };
                this.Disposing += (o,e) => { CharacterDisplay.Animated = false; CharacterDisplay.Visible = false; };
                //UpdateInterface();
            }

            private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (sender == null) return;
                if (e.KeyChar != (char)Keys.Enter) return;
                e.Handled = true;

                if (OKButton.Enabled)
                    OKButton.InvokeMouseClick(null);
            }
            private void CharacterNameTextBox_TextChanged(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    OKButton.Enabled = false;
                    NameTextBox.Border = false;
                }
                else if (!Reg.IsMatch(NameTextBox.Text))
                {
                    OKButton.Enabled = false;
                    NameTextBox.Border = true;
                    NameTextBox.BorderColour = Color.Red;
                }
                else
                {
                    OKButton.Enabled = true;
                    NameTextBox.Border = true;
                    NameTextBox.BorderColour = Color.Green;
                }
            }

            private void CreateCharacter()
            {
                OKButton.Enabled = false;

                Network.Enqueue(new C.NewCharacter
                {
                    Name = System.Text.Encoding.Default.GetBytes(NameTextBox.Text.Trim()),
                    Class = _class,
                    Gender = _gender,
                    Account = g_Account,
                    CharIndex = CharIndex
                });
            }

            private void UpdateInterface()
            {
                //
                MaleButton.Index = 77;//2420;
                FemaleButton.Index = 78;//2423;

                WarriorButton.Index = 74;//2426;
                WizardButton.Index = 75;//2429;
                TaoistButton.Index = 76;// 2432;
                //AssassinButton.Index = 2435;
                //ArcherButton.Index = 2438;

                switch (_gender)
                {
                    case MirGender.Male:
                        MaleButton.Index = 58;// 2421;
                        break;
                    case MirGender.Female:
                        FemaleButton.Index = 59;// 2424;
                        break;
                }

                switch (_class)
                {
                    case MirClass.Warrior:
                        WarriorButton.Index = 55;//2427;
                        Description.Text = WarriorDescription;
                        CharacterDisplay.Index = (byte)_gender == 0 ? 40 : 160;//20 : 300; //220 : 500;
                        break;
                    case MirClass.Wizard:
                        WizardButton.Index = 56;// 2430;
                        Description.Text = WizardDescription;
                        CharacterDisplay.Index = (byte)_gender == 0 ? 80 : 200;//40 : 320; //240 : 520;
                        break;
                    case MirClass.Taoist:
                        TaoistButton.Index = 57;//2433;
                        Description.Text = TaoistDescription;
                        CharacterDisplay.Index = (byte)_gender == 0 ? 120 : 240;//60 : 340; //260 : 540;
                        break;
                    //case MirClass.Assassin:
                    //    AssassinButton.Index = 2436;
                    //    Description.Text = AssassinDescription;
                    //    CharacterDisplay.Index = (byte)_gender == 0 ? 80 : 360; //280 : 560;
                    //    break;
                    //case MirClass.Archer:
                    //    ArcherButton.Index = 2439;
                    //    Description.Text = ArcherDescription;
                    //    CharacterDisplay.Index = (byte)_gender == 0 ? 100 : 140; //160 : 180;
                    //    break;
                }
                CharacterDisplay.Visible = true;
                CharacterDisplay.Animated = true;
                CharacterDisplay.Loop = true;
                //CharacterDisplay.Index = ((byte)_class + 1) * 20 + (byte)_gender * 280;
            }
        }
        public sealed class CharacterControl //: MirImageControl
        {
            public MirLabel NameLabel, LevelLabel, ClassLabel;
            public MirButton SelectButton;
            public bool Selected;
            public MirAnimatedControl CharacterDisplay;
            public bool IsEmpty;
            public byte CharIndex;
            public CharacterControl(MirControl parent)
            {
                SelectButton = new MirButton
                {
                    Index = 66,
                    HoverIndex = 66,
                    PressedIndex = 67,
                    Library = Libraries.Prguse,
                    Parent=parent

                };
                CharacterDisplay = new MirAnimatedControl
                {
                    Animated = false,
                    AnimationCount = 16,
                    AnimationDelay = 250,
                    FadeIn = true,
                    FadeInDelay = 75,
                    FadeInRate = 0.1F,

                    Index = 60,//stone Img Index unstone 40
                    Library = Libraries.ChrSel,
                    //Location = new Point(100,100),//new Point(200, 300),
                    Parent = parent,
                    //UseOffSet = true,
                    Visible = false,
                    Loop=false
                };
                CharacterDisplay.AfterDraw += (o, e) =>
                {
                    // if (_selected >= 0 && _selected < Characters.Count && characters[_selected].Class == MirClass.Wizard)
                    Libraries.ChrSel.DrawBlend(CharacterDisplay.Index + 560, CharacterDisplay.DisplayLocationWithoutOffSet, Color.White, true);
                };

                NameLabel = new MirLabel
                {
                    Location = new Point(107, 450),
                    Parent = parent,
                    NotControl = true,
                    Size = new Size(170, 18)
                };

                LevelLabel = new MirLabel
                {
                    Location = new Point(107, 480),
                    Parent = parent,
                    NotControl = true,
                    Size = new Size(30, 18)
                };

                ClassLabel = new MirLabel
                {
                    Location = new Point(107, 510),
                    Parent = parent,
                    NotControl = true,
                    Size = new Size(100, 18)
                };
            }


            public void Update(SelectInfo info)
            {
                CharacterDisplay.Animated = false;
                CharacterDisplay.Loop = false;
                CharacterDisplay.Visible = false;
                SelectButton.Index = 66;
                if (info == null)
                {

                    NameLabel.Text = string.Empty;
                    LevelLabel.Text = string.Empty;
                    ClassLabel.Text = string.Empty;

                    NameLabel.Visible = false;
                    LevelLabel.Visible = false;
                    ClassLabel.Visible = false;
                    SelectButton.Enabled = false;
                    Selected = false;
                    IsEmpty = true;
                    return;
                }
                /*new begin*/
                IsEmpty = false;
                int index = 0;
                switch (info.Class)
                {
                    case MirClass.Warrior:
                        index = info.Gender == 0 ? 60 : 180;//20 : 300; //220 : 500;
                        break;
                    case MirClass.Wizard:
                        index = info.Gender == 0 ? 100 : 220;//40 : 320; //240 : 520;
                        break;
                    case MirClass.Taoist:
                        index = info.Gender == 0 ? 140 : 260;//60 : 340; //260 : 540;
                        break;
                }
                CharacterDisplay.Index = index;
                SelectButton.Enabled = true;
                if (Selected)
                {
                    CharacterDisplay.Index = index-20;
                    CharacterDisplay.Animated = true;
                    CharacterDisplay.Loop = true;
                    SelectButton.Index = SelectButton.PressedIndex;
                    SelectButton.Enabled = false;
                }
                /*new end*/
                //Library = Libraries.Title;

                //Index = 660 + (byte)info.Class;

                //if (Selected) SelectButton.Index = SelectButton.PressedIndex;

                NameLabel.Text =new string(info.Name);
                LevelLabel.Text = info.Level.ToString();
                ClassLabel.Text = info.Class.ToString();

                CharacterDisplay.Visible = true;
                NameLabel.Visible = true;
                LevelLabel.Visible = true;
                ClassLabel.Visible = true;
                SelectButton.Visible = true;
            }
        }
    }
}
