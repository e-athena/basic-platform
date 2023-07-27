import { PieChartOutlined } from '@ant-design/icons';
import { ProCard, StatisticCard } from '@ant-design/pro-components';
import { Button, Card, Divider } from 'antd';
import React, { useState } from 'react';
import RcResizeObserver from 'rc-resize-observer';
import DemoLine from './components/DemoLine';
import DemoColumn from './components/DemoColumn';
import DemoPie from './components/DemoPie';
import { useModel } from '@umijs/max';

const Welcome: React.FC = () => {
  const [responsive, setResponsive] = useState(false);
  const [responsive1, setResponsive1] = useState(false);
  const { initialState } = useModel('@@initialState');
  const gridStyle: React.CSSProperties = {
    width: '33.3%',
    textAlign: 'center',
    cursor: 'pointer',
    padding: '16px 8px'
  };
  return (
    <RcResizeObserver
      key="resize-observer"
      onResize={(offset) => {
        setResponsive(offset.width < 596);
        setResponsive1(offset.width < 1070);
      }}
    >
      <ProCard.Group gutter={[16, 16]} style={{ backgroundColor: 'transparent' }} direction={responsive1 ? 'column' : 'row'}>
        <ProCard title={`欢迎回来，${initialState?.currentUser?.realName}`} headerBordered colSpan={responsive1 ? 24 : 17}>
          <StatisticCard.Group direction={responsive ? 'column' : 'row'}>
            <StatisticCard
              statistic={{
                title: '总流量(人次)',
                value: 601986875,
              }}
            />
            <Divider type={responsive ? 'horizontal' : 'vertical'} />
            <StatisticCard
              statistic={{
                title: '付费流量',
                value: 3701928,
                description: <StatisticCard.Statistic title="占比" value="61.5%" />,
              }}
              chart={
                <img
                  src="https://gw.alipayobjects.com/zos/alicdn/ShNDpDTik/huan.svg"
                  alt="百分比"
                  width="100%"
                />
              }
              chartPlacement="left"
            />
            <StatisticCard
              statistic={{
                title: '免费流量',
                value: 1806062,
                description: <StatisticCard.Statistic title="占比" value="38.5%" />,
              }}
              chart={
                <img
                  src="https://gw.alipayobjects.com/zos/alicdn/6YR18tCxJ/huanlv.svg"
                  alt="百分比"
                  width="100%"
                />
              }
              chartPlacement="left"
            />
          </StatisticCard.Group>
          {/* <Divider /> */}
          <Card title={'拆线图'}>
            <DemoLine />
          </Card>
        </ProCard>
        <ProCard colSpan={responsive1 ? 24 : 7}>
          <Card title="快捷访问" extra={<Button size={'middle'} type={'link'}>管理</Button>}>
            {[1, 2, 3, 4, 5, 6, 7, 8, 9].map((item) => (
              <Card.Grid key={item} style={gridStyle}>
                <PieChartOutlined style={{ fontSize: 22 }} />
                <div style={{ marginTop: 5 }}>用户管理</div>
              </Card.Grid>
            ))}
          </Card>
          <Divider />
          <Card title="最近访问">
            <Card.Grid style={gridStyle}>
              <PieChartOutlined style={{ fontSize: 22 }} />
              <div style={{ marginTop: 5 }}>用户管理</div>
            </Card.Grid>
            <Card.Grid style={gridStyle}>
              <PieChartOutlined style={{ fontSize: 22 }} />
              <div style={{ marginTop: 5 }}>用户管理</div>
            </Card.Grid>
            <Card.Grid style={gridStyle}>
              <PieChartOutlined style={{ fontSize: 22 }} />
              <div style={{ marginTop: 5 }}>用户管理</div>
            </Card.Grid>
          </Card>
        </ProCard>
      </ProCard.Group>
      <ProCard.Group gutter={[16, 16]} style={{ backgroundColor: 'transparent' }} direction={responsive1 ? 'column' : 'row'}>
        <ProCard title={'柱形图'} headerBordered colSpan={responsive1 ? 24 : 17}>
          <DemoColumn />
        </ProCard>
        <ProCard title={'最活跃员工'} subTitle={'最近30天前十的活跃度'} colSpan={responsive1 ? 24 : 7}>
          <DemoPie />
        </ProCard>
      </ProCard.Group>
    </RcResizeObserver>
  );
};

export default Welcome;
