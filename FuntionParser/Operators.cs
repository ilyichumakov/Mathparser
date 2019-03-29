using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace FuntionParser
{
    class Operators<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        public int Count => _items.Count;
        /// <param name="item"> Добавляемые данные. </param>
        public void Add(T item)
        {
            // Проверяем входные данные на пустоту.
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // Множество может содержать только уникальные элементы,
            // поэтому если множество уже содержит такой элемент данных, то не добавляем его.
            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }

        public void Remove(T item)
        {
            // Проверяем входные данные на пустоту.
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // Если коллекция не содержит данный элемент, то мы не можем его удалить.
            if (!_items.Contains(item))
            {
                throw new KeyNotFoundException($"Элемент {item} не найден в множестве.");
            }

            // Удаляем элемент из коллекции.
            _items.Remove(item);
        }

        public static Operators<T> Union(Operators<T> Operators1, Operators<T> Operators2)
        {
            // Проверяем входные данные на пустоту.
            if (Operators1 == null)
            {
                throw new ArgumentNullException(nameof(Operators1));
            }

            if (Operators2 == null)
            {
                throw new ArgumentNullException(nameof(Operators2));
            }

            // Результирующее множество.
            var resultOperators = new Operators<T>();

            // Элементы данных результирующего множества.
            var items = new List<T>();

            // Если первое входное множество содержит элементы данных,
            // то добавляем их в результирующее множество.
            if (Operators1._items != null && Operators1._items.Count > 0)
            {
                // т.к. список является ссылочным типом, 
                // то необходимо не просто передавать данные, а создавать их дубликаты.
                items.AddRange(new List<T>(Operators1._items));
            }

            // Если второе входное множество содержит элементы данных, 
            // то добавляем из в результирующее множество.
            if (Operators2._items != null && Operators2._items.Count > 0)
            {
                // т.к. список является ссылочным типом, 
                // то необходимо не просто передавать данные, а создавать их дубликаты.
                items.AddRange(new List<T>(Operators2._items));
            }

            // Удаляем все дубликаты из результирующего множества элементов данных.
            resultOperators._items = items.Distinct().ToList();

            // Возвращаем результирующее множество.
            return resultOperators;
        }

        public static Operators<T> Intersection(Operators<T> Operators1, Operators<T> Operators2)
        {
            // Проверяем входные данные на пустоту.
            if (Operators1 == null)
            {
                throw new ArgumentNullException(nameof(Operators1));
            }

            if (Operators2 == null)
            {
                throw new ArgumentNullException(nameof(Operators2));
            }

            // Результирующее множество.
            var resultOperators = new Operators<T>();

            // Выбираем множество содержащее наименьшее количество элементов.
            if (Operators1.Count < Operators2.Count)
            {
                // Первое множество меньше.
                // Проверяем все элементы выбранного множества.
                foreach (var item in Operators1._items)
                {
                    // Если элемент из первого множества содержится во втором множестве,
                    // то добавляем его в результирующее множество.
                    if (Operators2._items.Contains(item))
                    {
                        resultOperators.Add(item);
                    }
                }
            }
            else
            {
                // Второе множество меньше или множества равны.
                // Проверяем все элементы выбранного множества.
                foreach (var item in Operators2._items)
                {
                    // Если элемент из второго множества содержится в первом множестве,
                    // то добавляем его в результирующее множество.
                    if (Operators1._items.Contains(item))
                    {
                        resultOperators.Add(item);
                    }
                }
            }

            // Возвращаем результирующее множество.
            return resultOperators;
        }

        public static Operators<T> Difference(Operators<T> Operators1, Operators<T> Operators2)
        {
            // Проверяем входные данные на пустоту.
            if (Operators1 == null)
            {
                throw new ArgumentNullException(nameof(Operators1));
            }

            if (Operators2 == null)
            {
                throw new ArgumentNullException(nameof(Operators2));
            }

            // Результирующее множество.
            var resultOperators = new Operators<T>();

            // Проходим по всем элементам первого множества.
            foreach (var item in Operators1._items)
            {
                // Если элемент из первого множества не содержится во втором множестве,
                // то добавляем его в результирующее множество.
                if (!Operators2._items.Contains(item))
                {
                    resultOperators.Add(item);
                }
            }

            // Проходим по всем элементам второго множества.
            foreach (var item in Operators2._items)
            {
                // Если элемент из второго множества не содержится в первом множестве,
                // то добавляем его в результирующее множество.
                if (!Operators1._items.Contains(item))
                {
                    resultOperators.Add(item);
                }
            }

            // Удаляем все дубликаты из результирующего множества элементов данных.
            resultOperators._items = resultOperators._items.Distinct().ToList();

            // Возвращаем результирующее множество.
            return resultOperators;
        }

        public static bool SubOperators(Operators<T> Operators1, Operators<T> Operators2)
        {
            // Проверяем входные данные на пустоту.
            if (Operators1 == null)
            {
                throw new ArgumentNullException(nameof(Operators1));
            }

            if (Operators2 == null)
            {
                throw new ArgumentNullException(nameof(Operators2));
            }

            // Перебираем элементы первого множества.
            // Если все элементы первого множества содержатся во втором,
            // то это подмножество. Возвращаем истину, иначе ложь.
            var result = Operators1._items.All(s => Operators2._items.Contains(s));
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            // Используем перечислитель списка элементов данных множества.
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // Используем перечислитель списка элементов данных множества.
            return _items.GetEnumerator();
        }
    }
}
