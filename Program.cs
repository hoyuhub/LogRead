using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TXTCtrl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            HospStatistics(Read());
        }

        //需要指定文件路径（包括文件名称）
        public static List<Entity> Read()
        {
            string path = "e://20171011.txt";
            string line = string.Empty;
            List<Entity> list = new List<Entity>();
            //获取文件流
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                //去读文本信息，根据内容判断是否进行处理
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    var matches = Regex.Matches(sr.ReadToEnd(), @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}),\d{3} \[\d+] [A-Z]+ .* - Request .* (\{.*}) ([a-z]+|[a-z]+/[a-z]+)", RegexOptions.RightToLeft);
                    foreach (Match m in matches)
                    {
                        Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(m.Groups[2].Value);
                        string hospid = string.Empty;
                        string phone = string.Empty;
                        if (dic.Keys.Contains("request"))
                        {
                            Dictionary<string, string> dicRequest = JsonConvert.DeserializeObject<Dictionary<string, string>>(dic["request"].ToString());
                            if (dicRequest.Keys.Contains("hospid"))
                            {
                                hospid = dicRequest["hospid"];
                            }
                            if (dicRequest.Keys.Contains("phone"))
                            {
                                phone = dicRequest["phone"];
                            }
                        }
                        list.Add(new Entity(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, hospid, phone));
                    }
                }
            }
            return list;
        }

        public static void Ctrl(List<string[]> list)
        {

            var obj = from r in list group r by r[r.Length - 1];
        }

        //统计每家医院每秒调用次数
        public static void HospStatistics(List<Entity> list)
        {
            var s = from e in list group e by e.url;
            list.GroupBy(
                e => e.url,
                 (url, urlGroup) => new
                 {
                     url,
                     hospGroups =
                      urlGroup.GroupBy(
                        e2 => e2.hospid,
                         (hospid, hospGroup) => new
                         {
                             hospid,
                             times=hospGroup.Select(v=>v.time)
                         })
                 });
        }
    }

    //日志文件提炼后的行实体
    public class Entity
    {
        public Entity() { }
        public Entity(string time, string json, string url, string hospid, string phone)
        {
            this.time = time;
            this.json = json;
            this.url = url;
            this.hospid = hospid;
            this.phone = phone;
        }
        public string phone { get; set; }
        public string time { get; set; }
        public string hospid { get; set; }
        public string json { get; set; }
        public string url { get; set; }
    }

}
