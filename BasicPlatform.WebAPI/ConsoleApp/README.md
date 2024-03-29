# Athena Pro

This project is initialized with [Ant Design Pro](https://pro.ant.design). Follow is the quick guide for how to use.

## Environment Prepare

Install `node_modules`:

```bash
npm install
```

or

```bash
yarn
```

## Provided Scripts

Ant Design Pro provides some useful script to help you quick start and build with web project, code style check and test.

Scripts provided in `package.json`. It's safe to modify or add additional script:

### Start project

```bash
npm start
```

### Build project

```bash
npm run build
```

### Check code style

```bash
npm run lint
```

You can also use script to auto fix some lint error:

```bash
npm run lint:fix
```

### Test code

```bash
npm test
```

## More

You can view full document on our [official website](https://pro.ant.design). And welcome any feedback in our [github](https://github.com/ant-design/ant-design-pro).

## 使用 Tauri 打包客户端软件

### 1.安装 Rust 环境

> https://tauri.app/zh-cn/v1/guides/getting-started/prerequisites

### 2.安装 tauri-cli

> https://tauri.app/zh-cn/v1/guides/getting-started/setup/integrate

### 3.打包

```bash
npm run tauri build
# or
yarn tauri build
# or
pnpm tauri build
# or
cargo tauri build
```
