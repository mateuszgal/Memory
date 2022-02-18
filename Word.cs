namespace Memory
{
    class Word
    {

        private bool visible;
        private string content;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        public string Content
        {
            get { return content; } 
            set { content = value; }
        }
        public Word(string text)
        {
            content = text;
            Visible = false;
        }
    }

}