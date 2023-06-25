using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace HeroApp
{
    public partial class HeroCreator : Form
    {

        public HeroCreator()
        {
            InitializeComponent();
            Build();
            experienceNud.Maximum = uint.MaxValue;
            sss_Scroll(null!, null!);
            allianceTrk.Value = allianceTrk.Maximum / 2;
            allianceTrk_Scroll(null!, null!);
        }

        private void Build() {
            Builder.Abilities.Sort();
            Builder.Abilities.ForEach(s => specialAbilitiesFlPnl.Controls.Add(new CheckBox { Text = s }));
            
            Builder.Locations.Sort();
            locationsLst.Items.AddRange(Builder.Locations.ToArray<object>());
            locationsLst.SelectedIndex = 0;
            
            Builder.PreferredTransportation.ForEach(s => transportationFlPnl.Controls.Add(new RadioButton { Text = s }));
            transportationFlPnl.Controls.OfType<RadioButton>().First().Checked = true;

            Builder.PreferredWeapon.Sort();
            weaponsLst.Items.AddRange(Builder.PreferredWeapon.ToArray<object>());
            weaponsLst.SelectedIndex = 0;
        }

        private void HeroCreator_Load(object sender, EventArgs e)
        {

        }

        private void createHeroBtn_Click(object sender, EventArgs e) {
            if(CreateHero() == DialogResult.OK)
            {
                MessageBox.Show(@"Hero created successfully!", @"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private DialogResult CreateHero() {
            try {
                string heroName = heroNameTxt.Text;
                if (heroName.IsEmpty())
                    return ~MessageBox.Show(@"Please enter a name for your Hero!", @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                uint experience = (uint)experienceNud.Value;
                List< string > location = locationsLst.SelectedItems.Cast< string >().ToList();
                string transportation =
                    transportationFlPnl.Controls.OfType< RadioButton >().FirstOrDefault(r => r.Checked)!.Text;
                List< string > abilities = specialAbilitiesFlPnl.Controls.OfType< CheckBox >().Where(c => c.Checked)
                    .Select(c => c.Text).ToList();
                List< string > weapons = weaponsLst.SelectedItems.Cast< string >().ToList();
                Color favoriteColor = colorPicker.BackColor;
                int alliance = AllianceAdjust();
                string mug = mugShotPbx.Image.ToBase64();
                Dictionary< string, int > sss = new()
                    { { "speed", speedTrk.Value }, { "strength", strengthTrk.Value }, { "stamina", staminaTrk.Value } };
                var hero = new Hero(heroName, experience, location, transportation, abilities, weapons, favoriteColor,
                    alliance, mug, sss);
                hero.Save($"generatedHero{DateTime.Now:yyyyMMddhhmmss}.json");
            } catch (Exception ex)
            {
                return ~MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return DialogResult.OK;

        }

        private int AllianceAdjust() {
            return allianceTrk.Value - allianceTrk.Maximum / 2;
        }

        private void mugShotPbx_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new() {
                Filter =
                    @"Portable Network Graphics (*.png)|*.png|Joint Photographic Experts Group, err JPEG (*.jpg, *.jpeg)|*.jpg;*.jpeg|Bitmap (*.bmp, *.dib)|*.bmp;*.dib|All Files (*.*)|*.*"
            };
            if (ofd.ShowDialog(this) == DialogResult.OK) {
                mugShotPbx.Image = Image.FromFile(ofd.FileName);
            }
            mugShotPbx.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void colorPicker_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                colorPicker.BackColor = colorDialog.Color;
            }
            colorDialog.Dispose();
        }

        private void sss_Scroll(object sender, EventArgs e)
        {
            int speed = speedTrk.Value;
            int strength = strengthTrk.Value;
            int stamina = staminaTrk.Value;

            speedLbl.Text = $@"Speed: {speed}";
            strengthLbl.Text = $@"Strength: {strength}";
            staminaLbl.Text = $@"Stamina: {stamina}";

            int total = speed + strength + stamina;

            if (total < 100) return;

            int difference = total - 100;

            speedTrk.Value -= (int)((double)speed / total * difference);
            strengthTrk.Value -= (int)((double)strength / total * difference);
            staminaTrk.Value -= (int)((double)stamina / total * difference);
        }

        private void allianceTrk_Scroll(object sender, EventArgs e) {
            allianceLbl.Text = @$"Alliance: {AllianceAdjust()}";
        }

        private void showHeroesToolStripMenuItem_Click(object sender, EventArgs e) {
            new HeroDb().ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show(this, "Hero Creator made by Adonis S. Deliannis\n\nThis program is a part of the \"C# Programming II\" course\n at Grand Canyon University\n\n2023\n\nMy Malware scanner kept catching this program as malware because the AboutBox is polymorphic. So here we are, an ABOUT MESSAGE BOX...", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    internal record Hero(
        [JsonProperty("name")]string Name,
        [JsonProperty("experience")] uint Experience,
        [JsonProperty("location")] List<string> Location,
        [JsonProperty("transportation")] string Transportation,
        [JsonProperty("abilities")] List< string > Abilities,
        [JsonProperty("weapons")] List< string > Weapons,
        [JsonProperty("favorite_color")] Color FavoriteColor,
        [JsonProperty("alliance")] int Alliance,
        [JsonProperty("mug_shot")] string Mug,
        [JsonProperty("attributes")] Dictionary<string, int> Sss)
    {
        public void Save(string path) => File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        public static Hero? Load(string path) => !File.Exists(path) ? null : JsonConvert.DeserializeObject<Hero>(File.ReadAllText(path));

        public string Display()
        {
            string alliance = Alliance switch
            {
                < 0 => "an Evil",
                > 0 => "a Good",
                _ => "a very Neutral"
            };
            if(Location.Count > 1)
                Location[^1] = "and " + Location.Last();
            
            return $"Here we have {Name}, a {Experience} experienced hero, who has established residency in {string.Join(", ", Location)}. {Name} is a {Transportation} user, and is known for their {string.Join(", ", Abilities)} abilities. {Name} is also known to use {string.Join(", ", Weapons)} as weapons. {Name} is also known to be a {FavoriteColor.Name} lover. {Name} is also known to be {alliance} person. {Name} is also known to have {Sss["speed"]} speed, {Sss["strength"]} strength, and {Sss["stamina"]} stamina.";
        }

        #region Overrides of Object

        /// <inheritdoc />
        public override string ToString() {
            return Name;
        }

        #endregion
    }
}