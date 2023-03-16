using Athena.Infrastructure.DataPermission;
using BasicPlatform.AppService.Users.Responses;

namespace BasicPlatform.Test;

public class Test1
{
    [Test]
    public void Test2()
    {
        var res = DataPermissionHelper.GetStrategyTreeList("BasicPlatform.AppService");
        Assert.That(res[0].Label, Is.EqualTo("用户管理模块"));
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
    public void Test4()
    {
        var type = Type.GetType("BasicPlatform.AppService.Users.Responses.GetUserPagingResponse, BasicPlatform.AppService");

        Assert.Equals(type, typeof(GetUserPagingResponse));
        Assert.IsTrue(true);
    }
}