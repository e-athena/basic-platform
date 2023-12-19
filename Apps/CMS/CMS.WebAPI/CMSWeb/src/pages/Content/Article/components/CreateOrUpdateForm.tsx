import { submitHandle } from '@/utils/utils';
import { ProFormText, ProFormTextArea, ModalForm } from '@ant-design/pro-components';
import React from 'react';
import { update, create } from '../service';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.ArticleInfo;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  return (
    <ModalForm
      width={820}
      title={props.values?.id === undefined ? '添加文章' : '更新文章'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateArticleRequest) => {
        const isUpdate = props.values?.id !== undefined;
        let succeed;
        if (isUpdate) {
          values.id = props.values!.id!;
          succeed = await submitHandle(update, values);
        } else {
          succeed = await submitHandle(create, values as API.CreateArticleRequest);
        }
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props.values,
      }}
    >
      <ProFormText
        name="title"
        label="标题"
        placeholder="请输入标题"
        rules={[
          {
            required: true,
            message: '请输入标题',
          },
        ]}
      />
      <ProFormTextArea
        name="summary"
        label="摘要"
        placeholder="请输入摘要"
        fieldProps={{
          allowClear: true,
        }}
      />
      <ProFormTextArea
        name="content"
        label="内容"
        placeholder="请输入内容"
        fieldProps={{
          allowClear: true,
        }}
        rules={[
          {
            required: true,
            message: '请输入内容',
          },
        ]}
      />
      <ProFormText
        name="author"
        label="作者"
        placeholder="请输入作者昵称"
        rules={[
          {
            required: true,
            message: '请输入作者昵称',
          },
        ]}
      />
    </ModalForm>
  );
};

export default CreateOrUpdateForm;
