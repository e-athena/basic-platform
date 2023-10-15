using System.Reflection;
using System.Text.RegularExpressions;
using Athena.Infrastructure.Caching;
using Athena.Infrastructure.DataPermission;
using Athena.Infrastructure.EventTracking.Helpers;

namespace BasicPlatform.Test;

public class Test1 : TestBase
{
    [Test]
    public void Test00()
    {
        var res = EventTrackingHelper.GetEventSelectList(new List<Assembly>
        {
            Assembly.Load("BasicPlatform.Domain")
        });
        Assert.That(res, Is.Not.Empty);
        Assert.IsTrue(true);
    }

    [Test]
    public void Test0()
    {
        var res = EventTrackingHelper.GetEventTrackingTreeInfos(new List<Assembly>
        {
            Assembly.Load("BasicPlatform.AppService.FreeSql"),
            Assembly.Load("BasicPlatform.ProcessManager")
        });
        Assert.IsTrue(true);
    }

    [Test]
    public void Test2()
    {
        var res = DataPermissionHelper.GetGroupList("BasicPlatform.AppService", "test");
        var res2 = DataPermissionHelper.GetList("BasicPlatform.AppService", "test");
        var res3 = DataPermissionHelper.GetTreeList("BasicPlatform.AppService", "test");
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

        if (Regex.IsMatch(connectionString2, @"\b(MySQL|MariaDB)\b", RegexOptions.IgnoreCase))
        {
            Console.WriteLine("连接字符串2对应的数据库类型是 MySQL 或 MariaDB");
        }
        else if (Regex.IsMatch(connectionString2, @"\b(Data Source|Server)\b", RegexOptions.IgnoreCase))
        {
            Console.WriteLine("连接字符串2对应的数据库类型是 Microsoft SQL Server");
        }
        else
        {
            Console.WriteLine("连接字符串2对应的数据库类型未知");
        }

        Assert.IsTrue(true);
    }

    [Test]
    public void Test7()
    {
        var a = Provider.GetService<ICacheManager>();

        a?.Keys("user:640800b6054d3e000131af2c:*").ToList().ForEach(x => { Console.WriteLine(x); });

        RedisHelper.Instance.Keys("basic_platform:user:640800b6054d3e000131af2c:*").ToList().ForEach(x =>
        {
            Console.WriteLine(x);
        });

        Assert.Pass();
    }

    /// <summary>
    /// 根据时间读取本周周一的日期
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private static DateTime GetMonday(DateTime date)
    {
        var dt = date.DayOfWeek == DayOfWeek.Sunday ? date.AddDays(-1) : date;
        return dt.AddDays(-1 * (int) dt.DayOfWeek + 1);
    }

    /// <summary>
    /// 读取第几周
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private static int GetWeekOfYear(DateTime date)
    {
        var dt = date.DayOfWeek == DayOfWeek.Sunday ? date.AddDays(-1) : date;
        return (int) Math.Ceiling((double) dt.DayOfYear / 7);
    }

    /// <summary>
    /// 读取给定日期的周开始日期和结束日期
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private static Tuple<DateTime?, DateTime?> GetWeekRange(DateTime date)
    {
        return new Tuple<DateTime?, DateTime?>(GetMonday(date), date);
    }

    /// <summary>
    /// 读取上周的周一到周日的日期
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private static Tuple<DateTime?, DateTime?> GetLastWeekRange(DateTime date)
    {
        int startDay = date.DayOfWeek switch
        {
            DayOfWeek.Monday => -7,
            DayOfWeek.Tuesday => -8,
            DayOfWeek.Wednesday => -9,
            DayOfWeek.Thursday => -10,
            DayOfWeek.Friday => -11,
            DayOfWeek.Saturday => -12,
            DayOfWeek.Sunday => -13,
            _ => 0
        };

        var startDate = date.AddDays(startDay);
        var endDate = startDate.AddDays(6);

        return new Tuple<DateTime?, DateTime?>(startDate, endDate);
    }

    [Test]
    public void TestLastWeek()
    {
        var week1 = GetLastWeekRange(new DateTime(2023, 4, 10));
        var week2 = GetLastWeekRange(new DateTime(2023, 4, 11));
        var week3 = GetLastWeekRange(new DateTime(2023, 4, 12));
        var week4 = GetLastWeekRange(new DateTime(2023, 4, 13));
        var week5 = GetLastWeekRange(new DateTime(2023, 4, 14));
        var week6 = GetLastWeekRange(new DateTime(2023, 4, 15));
        var week7 = GetLastWeekRange(new DateTime(2023, 4, 16));
        var week8 = GetLastWeekRange(new DateTime(2023, 4, 8));
        var week9 = GetLastWeekRange(new DateTime(2023, 4, 9));
        Assert.Multiple(() =>
        {
            Assert.That(week2, Is.EqualTo(week1));
            Assert.That(week3, Is.EqualTo(week2));
            Assert.That(week4, Is.EqualTo(week3));
            Assert.That(week5, Is.EqualTo(week4));
            Assert.That(week6, Is.EqualTo(week5));
            Assert.That(week7, Is.EqualTo(week6));
            Assert.That(week9, Is.EqualTo(week8));
        });
        Assert.IsTrue(true);
    }

    [Test]
    public void TestWeek()
    {
        var week1 = GetWeekRange(new DateTime(2023, 4, 10));
        var week2 = GetWeekRange(new DateTime(2023, 4, 11));
        var week3 = GetWeekRange(new DateTime(2023, 4, 12));
        var week4 = GetWeekRange(new DateTime(2023, 4, 13));
        var week5 = GetWeekRange(new DateTime(2023, 4, 14));
        var week6 = GetWeekRange(new DateTime(2023, 4, 15));
        var week7 = GetWeekRange(new DateTime(2023, 4, 16));
        var week8 = GetWeekRange(new DateTime(2023, 4, 17));
        var week9 = GetWeekRange(new DateTime(2023, 4, 18));
        Assert.Multiple(() =>
        {
            Assert.That(week2.Item1, Is.EqualTo(week1.Item1));
            Assert.That(week3.Item1, Is.EqualTo(week2.Item1));
            Assert.That(week4.Item1, Is.EqualTo(week3.Item1));
            Assert.That(week5.Item1, Is.EqualTo(week4.Item1));
            Assert.That(week6.Item1, Is.EqualTo(week5.Item1));
            Assert.That(week7.Item1, Is.EqualTo(week6.Item1));
            Assert.That(week9.Item1, Is.EqualTo(week8.Item1));
        });
        Assert.IsTrue(true);
    }

    [Test]
    public void Test8()
    {
        for (var i = 0; i < 10; i++)
        {
            var str = $"QD-{DateTime.Now:yyyyMMddHHfff}";
            Console.WriteLine(str);
            Console.WriteLine(str.Length.ToString());
            Thread.Sleep(10);
        }

        Assert.IsTrue(true);
    }
}