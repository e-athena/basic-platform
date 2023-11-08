# 基础开发框架
> 基于.NET7开发的基础开发框架，包含基础的用户管理、角色管理、组织架构管理、职位管理、资源管理(菜单及操作)、数据权限管理、日志管理等功能，可作为基础开发框架使用。

## 特性
- 基于`.NET7`开发
- 前端使用`Ant Design Pro`，`UmiJs`
- 支持多租户
- 支持自动根据数据库连接字符串识别数据库类型。如：`sqlite,Data Source=test_local.db;`代表数据库类型为`sqlite`，连接字符串为`Data Source=test_local.db;`
- 支持两种ORM，`SqlSugar`和`FreeSql`，可根据需要自行选择。
- 支持自定义查询，可根据数据列表上的字段进行查询
- 支持列设置，可根据需要显示或隐藏数据列表上的字段
- 支持基于角色的授权(资源权限，数据权限，列权限，数据脱敏)。
- 支持基于用户的自定义授权(资源权限，数据权限，列权限，数据脱敏)。
- 数据权限支持基本授权和自定义授权两种方式。
  - 基本授权：支持`全部数据`、`本人数据`、`本部门数据`、`本部门及以下数据`、`自定义数据（部门）`。
  - 自定义授权：支持自定义数据权限，可根据需要自定义数据权限，可选字段为列表上的字段。
- 支持`领域事件`和`集成事件`，用于解耦业务逻辑和实现最终一致性。
- 支持实时通知，基于`SignalR`实现。

## 演示地址
[http://basic.zhengjinfan.cn](http://basic.zhengjinfan.cn)
> 帐号：root
> 密码：ant.design

## 启动步骤
1. 安装Redis
   > 本地安装Redis，或者使用Docker安装Redis
   > ```shell
   > docker run -d -p 6379:6379 --name redis redis
   > ```
2. cd到API目录下，执行`dotnet run`命令
   > 会自动创建数据库，如果数据库已存在，会自动迁移数据库
   > ```shell
   > cd BasicPlatform.WebAPI
   > dotnet run
   > ```
   > 如果需要更换数据库，可以在`appsettings.json`中修改`ConnectionStrings`
   >> 指定数据库连接字符串格式(Sqlite)如下：
   >> ```
   >> sqlite,Data Source=test_local.db;
   >> ```
   >> 框架能自动识别以上格式并自动设置数据库类型。 
   >> 格式说明：使用逗号分配，前一段为数据库类型，后一段为数据库连接字符串。支持的数据库类型有：sqlite、mysql、sqlserver、postgresql、oracle。
3. cd到Web目录下，执行`npm install`命令
    > 安装依赖包
    > ```shell
    > cd BasicPlatform.WebAPI/ConsoleApp
    > npm install
    > # or
    > yarn
    > # or
    > pnpm i # 推荐使用pnpm管理
    > ```
4. 执行`npm start`命令
5. 打开浏览器，访问`http://localhost:5079`
6. 输入帐号密码登录 
   > 帐号：root
   > 密码：ant.design

## QQ群
> 298736682

## 免责声明
- 仓库源码完全免费开源商用。
- 不能以任何形式用于非法为目的的行为。
- 任何基于本软件而产生的一切法律纠纷和责任，均于作者无关。
- 本软件不提供任何形式的明示或暗示担保，包括但不限于对软件的适用性、特定用途的适用性和非侵权性的担保。

## 鸣谢
![https://jb.gg/OpenSourceSupport](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg?_gl=1*ruggdh*_ga*MTcxMjE2Nzg1MS4xNjUxMDI5ODA0*_ga_9J976DJZ68*MTY5OTQ1MjM4MC4zOC4xLjE2OTk0NTMzNzQuNDIuMC4w&_ga=2.209954133.1323999414.1699367271-1712167851.1651029804)