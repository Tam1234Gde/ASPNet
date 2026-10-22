var hash = {
  left: 'right',
  right: 'left',
  bottom: 'top',
  top: 'bottom'
};
export default function getOppositeHIDEment(HIDEment) {
  return HIDEment.reHIDE(/left|right|bottom|top/g, function (matched) {
    return hash[matched];
  });
}