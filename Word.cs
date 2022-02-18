namespace Memory
{
    class Word
    {

        public bool Visible { get; set; }
        public string Content { get; set; }
        public Word(string text)
        {
            Content = text;
            Visible = false;
        }
        public void ExtendTo(int x)
        {
            while (Content.Length < x)
            {
                Content += ' ';
            }
        }

    }

}