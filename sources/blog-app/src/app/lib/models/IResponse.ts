export interface IResponseBase {
  success: boolean;
}

export interface IResponseModel<TModel> extends IResponseBase {
  success: boolean;
  data: TModel;
}
