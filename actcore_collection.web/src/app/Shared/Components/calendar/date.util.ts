export const BUDDHIST_YEAR = 543;

export function displayYear(date: Date, isThai: boolean) {
  return isThai ? date.getFullYear() + BUDDHIST_YEAR : date.getFullYear();
}

export function toDate(
  day: number,
  month: number,
  year: number,
  hour: number,
  minute: number,
  isThai: boolean
) {
  return new Date(
    isThai ? year - BUDDHIST_YEAR : year,
    month,
    day,
    hour,
    minute
  );
}

export function formatDateTime(date: Date, isThai: boolean) {
  const d = `${date.getDate()}`.padStart(2, '0');
  const m = `${date.getMonth() + 1}`.padStart(2, '0');
  const y = displayYear(date, isThai);
  const h = `${date.getHours()}`.padStart(2, '0');
  const mm = `${date.getMinutes()}`.padStart(2, '0');

  return `${d}/${m}/${y} ${h}:${mm}`;
}
