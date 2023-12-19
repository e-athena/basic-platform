# encoding=utf-8

import sys
import getopt
import os

# 前缀
image_prefix = 'registry.cn-shenzhen.aliyuncs.com/lrmtc/'

image_dict = {
    'web-api': image_prefix + 'basic-platform-web-api',
    'cms-api': image_prefix + 'e-demo-cms-api',
    'gateway': image_prefix + 'e-yarp-gateway',
}
path_dict = {
    'web-api': 'BasicPlatform.WebAPI',
    'cms-api': 'Apps/CMS/CMS.WebAPI',
    'gateway': 'BasicPlatform.Gateway'
}
proj_name_dict = {
    'web-api': 'BasicPlatform.WebAPI',
    'cms-api': 'CMS.WebAPI',
    'gateway': 'BasicPlatform.Gateway'
}


def main(argv):
    try:
        options, args = getopt.getopt(
            argv, "ht:n:", ["help", "name=", "tag="])
    except getopt.GetoptError:
        sys.exit()

    image_name = ""
    tag_name = "latest"
    path_name = ""
    proj_name = ""
    for option, value in options:
        if option in ("-h", "--help"):
            print("--help 或者 -h 显示帮助")
            print("--name 或者 -n 镜像名称")
            print("--tag 或者 -t 标签，默认值：latest")
            print("")
            print("-n host-api -t latest #构建 WebAPI")
        if option in ("-n", "--name"):
            image_name = image_dict[value]
            path_name = path_dict[value]
            proj_name = proj_name_dict[value]
        if option in ("-t", "--tag"):
            tag_name = value

    if image_name == '' or path_name == '':
        sys.exit()

    # # 本地 publish
    # publish_cmd = "dotnet publish -r linux-x64 \"./" + path_name + "/" + proj_name + ".csproj\" -c Release --no-self-contained"
    # os.system(publish_cmd)
    # # 执行命令
    # make_cmd = "make run image={0} tag={1} path={2}".format(image_name, tag_name, path_name)
    # os.system(make_cmd)

    # 本地 publish
    os.system(
        "dotnet publish -r linux-x64 \"./" + path_name + "/" + proj_name + ".csproj\" -c Release --no-self-contained")
    # 执行命令
    os.system("make run image={0} tag={1} path={2}".format(
        image_name, tag_name, path_name))


if __name__ == '__main__':
    main(sys.argv[1:])
