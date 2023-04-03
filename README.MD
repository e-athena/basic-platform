# 基础开发框架
> 基于.NET6开发的基础开发框架，包含基础的用户管理、角色管理、组织架构管理、职位管理、资源管理(菜单及操作)、数据权限管理、日志管理等功能，可作为基础开发框架使用。

## 特性
- 基于`.NET6`开发
- 前端使用`Ant Design Pro`，`UmiJs`
- 根据数据库连接字符串自动识别数据库类型，支持`Sqlite`、`MySql`、`SqlServer`、`PostgreSQL`等数据库
- 支持自定义查询，可根据数据列表上的字段进行查询
- 支持列设置，可根据需要显示或隐藏数据列表上的字段
- 支持基于角色的授权(资源权限，数据权限)。
- 支持基于用户的自定义授权(资源权限，数据权限)。
- 数据权限支持基本授权和自定义授权两种方式。
  - 基本授权：支持`全部数据`、`本人数据`、`本部门数据`、`本部门及以下数据`、`自定义数据（部门）`。
  - 自定义授权：支持自定义数据权限，可根据需要自定义数据权限，可选字段为列表上的字段。
- 支持`领域事件`和`集成事件`，用于解耦业务逻辑和实现最终一致性。

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
   > 如果需要指定数据库连接字符串，可以在`appsettings.json`中修改`ConnectionStrings`节点
   > ```json
   > "ConnectionStrings": {
   >    "Default": "Data Source=test_local.db;",
   >    "LoggerCenter": "Data Source=test_local_log.db;",
   >    "CAP": "127.0.0.1,connectTimeout=30000,keepAlive=60,syncTimeout=5000"
   > }
   > ```
3. cd到Web目录下，执行`npm install`命令
    > 安装依赖包
    > ```shell
    > cd BasicPlatform.WebAPI/ConsoleApp
    > npm install
    > # or
    > yarn
    > ```
4. 执行`npm start`命令
5. 打开浏览器，访问`http://localhost:5079`
6. 输入帐号密码登录 
   > 帐号：root
   > 密码：ant.design

## QQ群
> 298736682

## 免责声明
- 后端源码完全免费开源商用。
- 不能以任何形式用于非法为目的的行为。
- 任何基于本软件而产生的一切法律纠纷和责任，均于作者无关。
