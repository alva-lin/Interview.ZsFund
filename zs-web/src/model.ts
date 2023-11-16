export type MarketDataItem = {
  date: Date;
  price: number;
};

export type MarketEntity = {
  serialNumber: number;
  name: string;
  marketData: MarketDataItem[];
}

export type RelativeReturnItem = {
  date: Date;
  relativeReturn: number;
}

export type RelativeReturn = {
  entitySerialNumber: number;
  baseEntitySerialNumber: number;
  startDate: Date;
  endDate: Date;
  data: RelativeReturnItem[];
}
