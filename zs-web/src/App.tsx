import { Button, DatePicker, Select } from 'antd';
import axios from 'axios';
import dayjs from 'dayjs';
import ReactECharts from 'echarts-for-react';
import React, { useCallback, useEffect, useState } from 'react';
import './App.css';
import { MarketEntity, RelativeReturn, RelativeReturnItem } from './model';

const { RangePicker } = DatePicker;

const baseUrl = process.env.REACT_APP_API_PATH;

const DEFAULT_OPTION = {
  legend: {},
  tooltip: {},
  grid: { top: 8, right: 8, bottom: 24, left: 36 },
  series: [] as any[],
  xAxis: { type: 'time', name: '时间' },
  yAxis: {
    type: 'value',
    name: '相对收益',
    scale: true,
    axisTick: {
      length: 6,
    }
  },
}

function App() {
  // 获取数据
  const [ allStocks, setAllStocks ] = useState<MarketEntity[]>([]);
  useEffect(() => {
    axios.get<MarketEntity[]>(`${baseUrl}/MarketEntity`)
      .then(resp => {
        setAllStocks(resp.data);
      });
  }, []);

  // 设置下拉列表
  const [ selectedForA, setSelectedForA ] = useState<number[]>([]);
  const [ selectedForB, setSelectedForB ] = useState<number | null>(null);
  const optionsForA = allStocks.map(stock => ({
    label: stock.name,
    value: stock.serialNumber,
    disabled: selectedForB === stock.serialNumber // 禁用在B中已选的项
  }));

  const optionsForB = allStocks.map(stock => ({
    label: stock.name,
    value: stock.serialNumber,
    disabled: selectedForA.includes(stock.serialNumber) // 禁用在A中已选的所有项
  }));

  // 简单展示选中的项
  const selectedStocksForA = allStocks.filter(stock => selectedForA.includes(stock.serialNumber));
  const selectedStockForB = allStocks.find(stock => stock.serialNumber === selectedForB);

  // 选择时间
  const [ dates, setDates ] = useState<Date[]>([ new Date('2019-03-01'), new Date('2019-05-31') ]);

  // 获取相对收益
  const [ relativeReturns, setRelativeReturns ] = useState<RelativeReturn[]>([]);
  const getRelativeReturns = useCallback(() => {
    if (!selectedForA.length || selectedForB === null || dates.length !== 2) {
      return;
    }
    axios.post<RelativeReturn[]>(`${baseUrl}/MarketEntity/relative-return`, {
      serialNumbers: selectedForA,
      baseSerialNumber: selectedForB,
      startDate: dates[0],
      endDate: dates[1],
    }).then(resp => {
      setRelativeReturns(resp.data);
    })
  }, [ selectedForA, selectedForB, dates ]);
  useEffect(() => {
    getRelativeReturns();
  }, [ getRelativeReturns ]);

  // 更新图表
  const [ option, setOption ] = useState(DEFAULT_OPTION);
  useEffect(() => {
    if (relativeReturns.length === 0) {
      return;
    }

    const stocks = relativeReturns.map(relativeReturn => allStocks.find(stock => stock.serialNumber === relativeReturn.entitySerialNumber)!);

    const dimensions = [ 'date', ...stocks.map(stock => stock.name) ];
    const data: (RelativeReturnItem & { entitySerialNumber: number })[] = [];
    relativeReturns.forEach(relativeReturn => {
      relativeReturn.data.forEach(item => {
        data.push({
          entitySerialNumber: relativeReturn.entitySerialNumber,
          date: item.date,
          relativeReturn: item.relativeReturn,
        });
      });
    });

    const mergedData: { [key: string]: any }[] = [];
    data.forEach(item => {
      const date = item.date;
      const name = stocks.find(stock => stock?.serialNumber === item.entitySerialNumber)?.name;
      if (name) {
        const mergedItem = mergedData.find(mergedItem => mergedItem.date === date);
        if (mergedItem) {
          mergedItem[name] = item.relativeReturn;
        } else {
          mergedData.push({
            date,
            [name]: item.relativeReturn,
          });
        }
      }
    });

    const series = stocks.map(stock => ({
      type: 'line',
      id: stock.serialNumber!,
      name: stock.name,
      encode: {
        x: 'date',
        y: stock.name,
      }
    }));
    const newOption = {
      ...option,
      dataset: {
        dimensions,
        source: mergedData
      },
      series,
    };
    setOption(newOption);

  }, [ relativeReturns ]);

  return (
    <div className='App'>
      <div>
        {/* 下拉列表A（多选） */}
        <Select
          mode='multiple'
          style={{ width: '100%' }}
          placeholder='Select for A'
          value={selectedForA}
          onChange={setSelectedForA}
          options={optionsForA}
        />

        {/* 下拉列表B（单选） */}
        <Select
          style={{ width: '100%', marginTop: '10px' }}
          placeholder='Select for B'
          value={selectedForB}
          onChange={setSelectedForB}
          options={optionsForB}
        />
      </div>
      {/* 展示选中的选项 */}
      <div>
        <h3>Selected for A:</h3>
        <ul>
          {selectedStocksForA.map(stock => (
            <li key={stock.serialNumber}>{stock.name} ({stock.serialNumber})</li>
          ))}
        </ul>

        <h3>Selected for B:</h3>
        <p>{selectedStockForB ? `${selectedStockForB.name} (${selectedStockForB.serialNumber})` : 'None'}</p>
      </div>
      {/*选择时间*/}
      <div>
        <RangePicker allowClear
                     defaultValue={[ dayjs('2019-03-01'), dayjs('2019-05-31') ]}
                     onChange={(dates) => {
                       dates && dates[0] && dates[1] && setDates([ dates[0].toDate(), dates[1].toDate() ]);
                     }}/>
      </div>
      {/*刷新按钮*/}
      <div>
        <Button type={'default'} onClick={() => {
          getRelativeReturns();
        }}>刷新</Button>
      </div>
      {/*展示图表*/}
      <div>
        <ReactECharts option={option} notMerge={true}/>
      </div>
    </div>
  );
}

export default App;
