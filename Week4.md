# 操作与总结
>* 参考 Fantasy Skybox FREE 构建自己的游戏场景
>> 我所涉及的游戏场面主要有两个部分，第一个为天空盒`sky box`，另一个为游戏对象渲染。
>>* 天空盒
>>
>> 天空盒使用了师兄在某社交群上上传的`sky box`文件包，选择了天空盒`Think Cloud Water`，主要考虑的因素为这次试验做的是`牧师与魔鬼`过河，所以选择了有海水的场景。将文件夹导入`unity`。<br>
>>
>> 天空盒中有6张贴图。对应6个面。首先将`main camera`的`Clear flag`属性设置为`sky box`，并添加'Component'->`Randering`->'skybox'。<br>
>>
>> 创建一个新的`material`，选择'sky box'->`six sides`，按照提示顺序将6张图片拖入到`material`中。创建完成后将`material`拖入到`main camera`的`sky box`属性中。<br>
>>
![skyBox](http://imglf6.nosdn.127.net/img/S3F1ejdrdGNrNFVwaVBPZEJwSHVnakZsUWwrYldidFQ2SXpXMGlkOHJvS3ZiSk12UmtBYmNnPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0 "skyBox")<br>
![mainCamera](http://imglf5.nosdn.127.net/img/S3F1ejdrdGNrNFVwaVBPZEJwSHVnbU92TlFoWkh4dC95K0JnTk9rcWFZeGpEUkVnZUZTVFN3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0 "mainCamera")<br>
>>* 游戏对象<br>
>>
>> 在[unity asset store](https://assetstore.unity.com/)上下载石头，恶魔，牧师，船只，树木等游戏对象，替换之前的游戏对象。<br>
![替换对象](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFVwaVBPZEJwSHVnalVDVThQVDBUdFZoNDUvT1B5SjViZkRxcVlQc05MMEx3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0 "替换对象")<br>
>* 写一个简单的总结，总结游戏对象的使用
>>* 1.游戏对象
>>
>> 游戏对象是在游戏中创建的一个物体，从场景中的角色，环境，甚至背景音乐，照相机，都是游戏对象（当然，还有特殊的空对象）。<br>
>>
>>游戏对象被添加到游戏中时本身并不包含任何特性（特征，属性..）。当然，它们包含有已经被实现的控制组件。例如，一个灯光（Light）就是一个添加到游戏对象的组件。我们可以给游戏对象添加属性来控制游戏对象的运动。<br>
>>
>>* 
# 编程实践
