using System;
using TestTelegramBot.Models;
using TestTelegramBot.ServicesAbstraction;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace TestTelegramBot.Services
{
    public class FetchService : IFetchService
    {
        public string reference = "https://sbis.ru/contragents/";
        public string referenceEgrul = "https://выставить-счет.рф/vipiska-egrul/";
        public async Task<string> GetNameAndAddress(string innAll)
        {
            SplitStringsService splitString = new SplitStringsService();
            HttpClient httpClient = new HttpClient();
            HtmlDocument htmlDocument = new HtmlDocument();
            string result = string.Empty;

            List<string> inns = splitString.SplitInn(innAll);

            foreach (string inn in inns)
            {
                CompanyInfo companyInfo = new CompanyInfo();
                companyInfo.Inn = inn;
                try
                {
                    string htmlContent = await httpClient.GetStringAsync($"{reference}/{inn}");
                    htmlDocument.LoadHtml(htmlContent);

                    var node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='cCard__MainReq-Name']");
                    if (node == null)
                    {
                        return result;
                    }
                    node = node.FirstChild;
                    if (node != null) companyInfo.CompanyName = node.InnerText;
                    node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='cCard__Contacts-Address']");
                    if (node != null) companyInfo.CompanyAddress = node.InnerText;
                    if ((companyInfo.CompanyName != string.Empty & companyInfo.CompanyName != null) && (companyInfo.CompanyAddress != string.Empty & companyInfo.CompanyAddress != null))
                    {
                        result = string.Concat(result, $"ИНН компании: {companyInfo.Inn}.\n" +
                                                       $"Название компании: {companyInfo.CompanyName}.\n" +
                                                       $"Адрес компании: {companyInfo.CompanyAddress}.\n\n");
                    }
                    else
                    {
                        result = string.Concat(result, $"Не получилось получить информацию по ИНН: {companyInfo.Inn}. Пожалуйста, перепроверьте ИНН или попробуйте позже.\n\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    result = string.Concat(result, $"Не получилось получить информацию по ИНН: {companyInfo.Inn}. Пожалуйста, перепроверьте ИНН или попробуйте позже.\n\n");
                }
                Thread.Sleep(2000); //Предосторожность чтобы сайт с информацией на заблокировал ИП сервера по причине подозрительной активности
            }

            return result;
        }
        public async Task<string> GetOgrn(string inn)
        {
            HttpClient httpClient = new HttpClient();
            HtmlDocument htmlDocument = new HtmlDocument();
            CompanyInfo companyInfo = new CompanyInfo();
            companyInfo.Inn = inn;
            try {
                string htmlContent = await httpClient.GetStringAsync($"{reference}/{inn}");
                htmlDocument.LoadHtml(htmlContent);

                var node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='cCard__CompanyDescription']");


                if (node == null) return $"Не получилось получить информацию о компании по ИНН: {companyInfo.Inn}. Пожалуйста, перепроверьте ИНН или попробуйте позже.";

                string innerHtml = node.InnerHtml;

                string pattern = @"ОГРН (\d+)";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(innerHtml);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                else
                {
                    return $"Удалось получить данные о компании, но не получилось узнать ОРГН по ИНН: {companyInfo.Inn}. Пожалуйста, попробуйте позже.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return $"Не получилось получить информацию по ИНН: {companyInfo.Inn}. Пожалуйста, перепроверьте ИНН или попробуйте позже.";
            }
        }
        public string GetEgrulLink(string ogrn)
        {
            string egrulLink = $"{referenceEgrul}{ogrn}.pdf";
            return egrulLink;
        }
    }
}
