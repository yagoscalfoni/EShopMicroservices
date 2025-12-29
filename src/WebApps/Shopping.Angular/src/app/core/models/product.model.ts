export interface Product {
  id: string;
  name: string;
  category: string[];
  description: string;
  imageFile: string;
  price: number;
}

export interface GetProductsResponse {
  products: Product[];
}

export interface GetProductByCategoryResponse {
  products: Product[];
}

export interface GetProductByIdResponse {
  product: Product;
}
