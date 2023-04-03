# 基础开发框架
> 集成`资源权限`和`数据权限`功能

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