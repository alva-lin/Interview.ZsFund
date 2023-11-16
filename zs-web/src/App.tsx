import { Select } from 'antd';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import './App.css';

type MarketDataItem = {
  date: Date;
  price: number;
};

type MarketEntity = {
  serialNumber: number;
  name: string;
  marketData: MarketDataItem[];
}

type RelativeReturnItem = {
  date: Date;
  relativeReturn: number;
}

type RelativeReturn = {
  entitySerialNumber: number;
  baseEntitySerialNumber: number;
  startDate: Date;
  endDate: Date;
  data: RelativeReturnItem[];
}

const baseUrl = "https://localhost:7148";

function App() {
  // 获取数据
  const [ stocks, setStocks ] = useState<MarketEntity[]>([]);
  useEffect(() => {
    axios.get<MarketEntity[]>(`${baseUrl}/MarketEntity`)
      .then(resp => {
        console.log(resp)
        setStocks(resp.data);
      });
  }, []);

  // 设置下拉列表
  const [ selectedForA, setSelectedForA ] = useState<number[]>([]);
  const [ selectedForB, setSelectedForB ] = useState<number | null>(null);
  const optionsForA = stocks.map(stock => ({
    label: stock.name,
    value: stock.serialNumber,
    disabled: selectedForB === stock.serialNumber // 禁用在B中已选的项
  }));

  const optionsForB = stocks.map(stock => ({
    label: stock.name,
    value: stock.serialNumber,
    disabled: selectedForA.includes(stock.serialNumber) // 禁用在A中已选的所有项
  }));

  // 简单展示选中的项
  const selectedStocksForA = stocks.filter(stock => selectedForA.includes(stock.serialNumber));
  const selectedStockForB = stocks.find(stock => stock.serialNumber === selectedForB);
  
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
    </div>
  );
}

export default App;
