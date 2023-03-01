using OpenAI_API;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Bot
{
    public partial class AIBotApp : Form
    {
        public static bool CanSendMessage = true;

        public AIBotApp()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += textBox1.Text + Environment.NewLine;
            var ChatGPT = new OpenAIChatGPT("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");
            string input = textBox1.Text;
            textBox1.Text = "";
            button1.Enabled = false;
            CanSendMessage = false;
            string response = await ChatGPT.GenerateResponse(input);
            button1.Enabled = true;
            CanSendMessage = true;

            richTextBox1.Text += "    " + response.Trim() + Environment.NewLine + Environment.NewLine;
            textBox1.Focus();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            textBox1.Focus();
            CanSendMessage = true;
            richTextBox1.Text = "Hello! I am an AI program derived from the AI module GPT-3,\nand I will try to answer whatever questions you may have.\nI am especially good at helping create, debug, and fix code.\n\n";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("\n"))
            {
                textBox1.Text = textBox1.Text.Trim();
                textBox1.Select(Right,0);
                if (CanSendMessage)
                {
                    button1_Click(sender, e);
                }
            }
        }
    }
}

public class OpenAIChatGPT
{
    private readonly OpenAI_API.OpenAIAPI _api;

    public OpenAIChatGPT(string apiKey)
    {
        _api = new OpenAI_API.OpenAIAPI(apiKey);
    }

    public async Task<string> GenerateResponse(string prompt)
    {
        try
        {
            var completions = await _api.Completions.CreateCompletionAsync(
            prompt: prompt,
            max_tokens: 2048,
            model: OpenAI_API.Models.Model.DavinciText,
            numOutputs: 5,
            stopSequences: null,
            temperature: 0.4,
            frequencyPenalty: -15,
            echo: false);

            var response = completions.Completions[0].Text;
            return response;
        }
        catch (Exception ex)
        {
            return "Sorry, something went wrong.\nError Code: "+ex.Message;
        }
    }
}
