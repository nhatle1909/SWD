export const linkImg = (name) => {
    return new URL(`../assets/images/${name}`, import.meta.url).href
}

export const getLocalStorage = (name) => {
  const data = localStorage.getItem(name);
  if(data){
    return JSON.parse(data)
  }
  return null;
}

export const setLocalStorage = (name, data) => {
  localStorage.setItem(name, JSON.stringify(data));
}