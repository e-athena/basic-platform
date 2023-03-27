using System.Text.RegularExpressions;
using BasicPlatform.AppService.DataPermissions;

namespace BasicPlatform.Test;

public class Test1
{
    [Test]
    public void Test2()
    {
        var res = DataPermissionHelper.GetTreeList("BasicPlatform.AppService");
        var res2 = DataPermissionHelper.GetList("BasicPlatform.AppService");
        Assert.IsTrue(true);
    }

    [Test]
    public void Test3()
    {
        var userAgents = new List<string>
        {
            "Mozilla/5.0 (Linux; Android 9; FIG-AL10 Build/HUAWEIFIG-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/105.0.0.0MQQBrowser/6.2 TBS/045223 Mobile Safari/537.36 MMWEBID/1214 MicroMessenger/7.0.14.1660(0x27000E39) Process/tools NetType/WIFI Language/zh_CN ABI/arm64 WeChat/arm64 wechatdevtools qcloudcdn-xinan",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36 Edg/110.0.1587.69"
        };

        foreach (var userAgent in userAgents)
        {
            var uap = UAParser.Parser.GetDefault().Parse(userAgent);

            // 获取浏览器信息
            var browserName = uap.UA.Family;
            var browserVersion = $"{uap.UA.Major}.{uap.UA.Minor}.{uap.UA.Patch}";

            // 获取操作系统信息
            var osName = uap.OS.Family;
            var osVersion = $"{uap.OS.Major}.{uap.OS.Minor}.{uap.OS.Patch}";

            // 获取设备信息
            var deviceName = uap.Device.Family;
            var deviceBrand = uap.Device.Brand;
            var deviceModel = uap.Device.Model;

            Console.WriteLine($"浏览器名称：{browserName}");
            Console.WriteLine($"浏览器版本号：{browserVersion}");
            Console.WriteLine($"操作系统名称：{osName}");
            Console.WriteLine($"操作系统版本号：{osVersion}");
            Console.WriteLine($"设备名称：{deviceName}");
            Console.WriteLine($"设备品牌：{deviceBrand}");
            Console.WriteLine($"设备型号：{deviceModel}");
            Console.WriteLine("=====================================");
        }

        Assert.IsTrue(true);
    }

    [Test]
    public void Test5()
    {
        var list = GetMonthList(DateTime.Parse("2022-1-3"), DateTime.Parse("2023-7-4"));
        Assert.Multiple(() =>
        {
            Assert.That(list[0], Is.EqualTo("2022-01"));
            Assert.That(list[1], Is.EqualTo("2022-02"));
        });
        Assert.IsTrue(true);
    }

    // 根据开始和结束时间获取月份列表
    private static List<string> GetMonthList(DateTime startTime, DateTime endTime)
    {
        var list = new List<string>();
        var startMonth = startTime.ToString("yyyy-MM");
        var endMonth = endTime.ToString("yyyy-MM");
        if (startMonth == endMonth)
        {
            list.Add(startMonth);
            return list;
        }

        var startYear = startTime.Year;
        var startMonthNum = startTime.Month;
        var endYear = endTime.Year;
        var endMonthNum = endTime.Month;
        for (var i = startYear; i <= endYear; i++)
        {
            var start = i == startYear ? startMonthNum : 1;
            var end = i == endYear ? endMonthNum : 12;
            for (var j = start; j <= end; j++)
            {
                list.Add($"{i}-{j:D2}");
            }
        }

        return list;
    }

    [Test]
    public void Test6()
    {
        string connectionString1 =
            "data source=rm-bp1rr35xj61hyv709ko.mysql.rds.aliyuncs.com;port=3306;database=test_db; uid=athena_test;pwd=123123123;charset=utf8mb4;ssl mode=none;";
        string connectionString2 =
            "Server=183.6.120.119;Initial Catalog=ResumeSystem;User ID=sa;Password=Dianmi123;MultipleActiveResultSets=true;";

        // 使用正则表达式判断数据库类型
        if (Regex.IsMatch(connectionString1, @"\b(MySQL|MariaDB)\b", RegexOptions.IgnoreCase))
        {
            Console.WriteLine("连接字符串1对应的数据库类型是 MySQL 或 MariaDB");
        }
        else if (Regex.IsMatch(connectionString1, @"\b(Data Source|Server)\b", RegexOptions.IgnoreCase))
        {
            Console.WriteLine("连接字符串1对应的数据库类型是 Microsoft SQL Server");
        }
        else
        {
            Console.WriteLine("连接字符串1对应的数据库类型未知");
        }

        if (Regex.IsMatch(connectionString2, @"\b(Data Source|Server)\b", RegexOptions.IgnoreCase))
        {
            Console.WriteLine("连接字符串2对应的数据库类型是 Microsoft SQL Server");
        }
        else if (Regex.IsMatch(connectionString2, @"\b(MySQL|MariaDB)\b", RegexOptions.IgnoreCase))
        {
            Console.WriteLine("连接字符串2对应的数据库类型是 MySQL 或 MariaDB");
        }
        else
        {
            Console.WriteLine("连接字符串2对应的数据库类型未知");
        }
        Assert.IsTrue(true);
    }
}