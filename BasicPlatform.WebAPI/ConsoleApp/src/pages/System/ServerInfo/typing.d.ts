declare namespace API {
  /** 服务器信息 */
  type ServerInfo = {
    appName: string;
    appVersion: string;
    osArchitecture: string;
    osDescription: string;
    processArchitecture: string;
    basePath: string;
    runtimeFramework: string;
    frameworkDescription: string;
    hostName: string;
    ipAddress: string;
    processName: string;
    virtualMemory: string;
    memoryUsage: string;
    startTime: string;
    threads: number;
    totalProcessorTime: string;
    userProcessorTime: string;
    environments: {
      [key: string]: string;
    };
  };
}
