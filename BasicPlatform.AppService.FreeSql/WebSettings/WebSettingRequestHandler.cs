using BasicPlatform.AppService.WebSettings.Requests;
using BasicPlatform.Domain.Models.WebSettings;

namespace BasicPlatform.AppService.FreeSql.WebSettings;

/// <summary>
/// 网站设置请求处理程序
/// </summary>
public class WebSettingRequestHandler : ServiceBase<WebSetting>,
    IRequestHandler<SaveWebSettingRequest, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="contextAccessor"></param>
    public WebSettingRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor contextAccessor)
        : base(unitOfWorkManager, contextAccessor)
    {
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(SaveWebSettingRequest request, CancellationToken cancellationToken)
    {
        var entity = await Queryable.FirstAsync(cancellationToken);
        if (entity == null)
        {
            entity = new WebSetting(
                request.Logo,
                request.Ico,
                request.Name,
                request.ShortName,
                request.Description,
                request.CopyRight
            );
            await RegisterNewAsync(entity, cancellationToken);
            return entity.Id;
        }

        entity.Update(
            request.Logo,
            request.Ico,
            request.Name,
            request.ShortName,
            request.Description,
            request.CopyRight
        );
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}