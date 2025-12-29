import { environment } from '../../../environments/environment';

export const PRODUCT_PLACEHOLDER = (label = 'Produto') =>
  `https://via.placeholder.com/640x480?text=${encodeURIComponent(label)}`;

export function productImageUrl(imageFile?: string, label = 'Produto'): string {
  const candidate = imageFile?.trim();

  if (!candidate) {
    return PRODUCT_PLACEHOLDER(label);
  }

  if (candidate.startsWith('http')) {
    return candidate;
  }

  return `${environment.apiBaseUrl}/images/product/${candidate}`;
}
