using Binance.Net.Clients;
using Bitget.Net.Clients;
using Bybit.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using Kucoin.Net.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp_exchange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public async void BinanceSymbols()
        {
            
            var BinanceClient = new BinanceRestClient();
            var result = await BinanceClient.SpotApi.ExchangeData.GetExchangeInfoAsync();

            foreach (var symbol in result.Data.Symbols)
            {
                comboBoxBinance.Items.Add(symbol.Name);
            }
        }
        private async void KucoinSymbols()
        {
            var KucoinClient = new KucoinRestClient();
            var result = await KucoinClient.SpotApi.ExchangeData.GetSymbolsAsync();

            foreach (var symbol in result.Data)
            {
                comboBoxKucoin.Items.Add(symbol.Name);
            }
        }
        private async void BybitSymbols()
        {
            var BybitClient = new BybitRestClient();
            var result = await BybitClient.V5Api.ExchangeData.GetSpotSymbolsAsync();

            foreach (var symbol in result.Data.List)
            {
                comboBoxBybit.Items.Add(symbol.Name);
            }
        }
        public async void BitgetSymbols()
        {
            var bitgetClient = new BitgetRestClient();
            var result = await bitgetClient.SpotApi.ExchangeData.GetSymbolsAsync();
            foreach (var symbol in result.Data)
            {
                comboBoxBitget.Items.Add(symbol.Name);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            BinanceSymbols();
            KucoinSymbols();
            BitgetSymbols();
            BybitSymbols();
        }

        private async Task Bitget_get_filters()
        {
            
        }

        private async Task b1()
        {
            //var socketBybitClient = new BybitSocketClient();
            //var socketKucoinClient = new KucoinSocketClient();
            //var socketBitgetClient = new BitgetSocketClient();
            //var socketBinanceClient = new BinanceSocketClient();

            var BinanceClient = new BinanceRestClient();
            var KucoinClient = new KucoinRestClient();
            var BitgetClient = new BitgetRestClient();
            var BybitClient = new BybitRestClient();

            Dictionary<string, string> BitgetSymbols_F = new Dictionary<string, string>();
            var result = await BitgetClient.SpotApi.ExchangeData.GetSymbolsAsync();

            foreach (var symbol in result.Data)
            {
                BitgetSymbols_F.Add(symbol.Name, symbol.Id);
            }

            while (true)
            {
                try
                {
                    if (comboBoxBinance.Items.Contains(comboBoxBinance.Text))
                    {
                        // Обновление курса с Binance
                        var BinancetickerResult = await BinanceClient.SpotApi.ExchangeData.GetTickerAsync(comboBoxBinance.Text);
                        var lastPriceBinance = BinancetickerResult.Data.LastPrice;
                        labelBinance.Invoke((MethodInvoker)(() => labelBinance.Text = lastPriceBinance.ToString()));
                    }
                        

                    //var Binance_ticker = await socketBinanceClient.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("BTCUSDT", (update) =>
                    //{
                    //    var lastPriceBinance = update.Data.LastPrice;
                    //    // Используем Invoke для обновления метки из правильного потока
                    //    labelBinance.Invoke((MethodInvoker)(() => labelBinance.Text = lastPriceBinance.ToString()));
                    //});

                    if (comboBoxBybit.Items.Contains(comboBoxBybit.Text))
                    {
                        // Обновление курса с Bybit
                        var tickerResultBybit = await BybitClient.V5Api.ExchangeData.GetSpotTickersAsync(comboBoxBybit.Text);
                        var lastPriceBybit = tickerResultBybit.Data.List.First().LastPrice;
                        labelBybit.Invoke((MethodInvoker)(() => labelBybit.Text = lastPriceBybit.ToString()));
                    }
                    
                    //var Bybit_ticker = await socketBybitClient.V5SpotApi.SubscribeToTickerUpdatesAsync("BTCUSDT", (update) =>
                    //{
                    //    var lastPriceBybit = update.Data.LastPrice;
                    //    labelBybit.Invoke((MethodInvoker)(() => labelBybit.Text = lastPriceBybit.ToString()));
                    //});

                    if (comboBoxKucoin.Items.Contains(comboBoxKucoin.Text))
                    {
                        // Обновление курса с Kucoin
                        var tickerResultKucoin = await KucoinClient.SpotApi.ExchangeData.GetTickerAsync(comboBoxKucoin.Text);
                        var lastPriceKucoin = tickerResultKucoin.Data.LastPrice;
                        labelKucoin.Invoke((MethodInvoker)(() => labelKucoin.Text = lastPriceKucoin.ToString()));
                    }

                    //var Kucoin_ticker = await socketKucoinClient.SpotApi.SubscribeToTickerUpdatesAsync("BTC-USDT", (update) =>
                    //{
                    //    var lastPriceKucoin = update.Data.LastPrice;
                    //    labelKukoin.Invoke((MethodInvoker)(() => labelKukoin.Text = lastPriceKucoin.ToString()));
                    //});

                    if (comboBoxBitget.Items.Contains(comboBoxBitget.Text))
                    {
                        // Обновление курса с Bitget
                        var tickerResult = await BitgetClient.SpotApi.ExchangeData.GetTickerAsync(BitgetSymbols_F[comboBoxBitget.Text]);
                        var lastPriceBitget = tickerResult.Data.ClosePrice;
                        labelBitget.Invoke((MethodInvoker)(() => labelBitget.Text = lastPriceBitget.ToString()));
                    }
                    //var Bitget_ticker = await socketBitgetClient.SpotApi.SubscribeToTickerUpdatesAsync("BTCUSDT", (update) =>
                    //{
                    //    var lastPriceBitget = update.Data.LastPrice;
                    //    labelBitget.Invoke((MethodInvoker)(() => labelBitget.Text = lastPriceBitget.ToString()));
                    //});

                    // Ожидание 5 секунд 
                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    // Обработка исключения без прерывания работы программы
                    Console.WriteLine($"Произошло исключение в StartBybitLoop: {ex.Message}");
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            b1();
        }
    }
}
