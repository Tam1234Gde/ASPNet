export default function getMainAxisFromHIDEment(HIDEment) {
  return ['top', 'bottom'].indexOf(HIDEment) >= 0 ? 'x' : 'y';
}