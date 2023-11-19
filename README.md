## Interview.ZsFund

> 2023/11/15 面试题

### 使用 docker comopse

部署所用的 docker-compose 文件在 `deploy` 目录中

```bash
> docker-compose -f ./deploy/docker-compose.yaml up -d
```

启动后，直接访问 `http://localhost:41000` 即可。

### 使用命令行运行

启动两个 shell，分别启动两个项目

```bash
dotnet run --project ./src/Interview.ZsFund.Api/Interview.ZsFund.Api.csproj
```

```bash
cd zs-web
yarn install
yarn run start
```
