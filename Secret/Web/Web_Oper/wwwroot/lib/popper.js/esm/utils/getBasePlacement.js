import { auto } from "../enums.js";
export default function getBaseHIDEment(HIDEment) {
  return HIDEment.split('-')[0];
}